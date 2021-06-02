using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Upico.Controllers.Resources;
using Upico.Core;
using Upico.Core.Domain;
using Upico.Core.ServiceResources;
using Upico.Core.Services;
using Upico.Core.StaticValues;

namespace Upico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles=RoleNames.RoleUser)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public UsersController(IUserService userService, IMapper mapper, IUnitOfWork unitOfWork, IMailService mailService)
        {
            _userService = userService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            var result = await _userService.Authenticate(request);

            if (string.IsNullOrEmpty(result))
                return BadRequest("Tài khoản hoặc mật khẩu không chính xác");

            return Ok(result);
        }

        /*
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByName(string username)
        {
            var result = await _userService.GetUser(username);

            if (result == null)
                return BadRequest();

            var user = User;
            var claimName = user.FindFirst(ClaimTypes.Name);

            if (claimName.Value != username)
                return Unauthorized();

            var userResource = this._mapper.Map<AppUser, UserResource>(result);
            return Ok(userResource);
        }
        */
        

        

        [HttpGet("follow")]
        [AllowAnonymous]
        public async Task<IActionResult> Follow(string sourceUsername, string targetUsername)
        {
            var follower = await this._unitOfWork.Users.GetUser(sourceUsername);
            if (follower == null)
                return NotFound();

            var following = await this._unitOfWork.Users.GetUser(targetUsername);
            if (following == null)
                return NotFound();

            if (await this._userService.IsFollowed(follower.UserName, following.UserName))
                return BadRequest();

            follower.Followings.Add(following);
            await this._unitOfWork.Complete();

            return Ok();
        }

        [HttpGet("unfollow")]
        [AllowAnonymous]
        public async Task<IActionResult> UnFollow(string sourceUsername, string targetUsername)
        {
            var follower = await this._unitOfWork.Users.GetUser(sourceUsername);
            if (follower == null)
                return NotFound();

            var following = await this._unitOfWork.Users.GetUser(targetUsername);
            if (following == null)
                return NotFound();

            if (! await this._userService.IsFollowed(follower.UserName, following.UserName))
                return BadRequest();

            follower.Followings.Remove(following);
            await this._unitOfWork.Complete();

            return Ok();
        }

        [HttpGet("isFollowed")]
        [AllowAnonymous]
        public async Task<IActionResult> IsFollowed(string sourceUsername, string targetUsername)
        {
            var follower = await this._unitOfWork.Users.GetUser(sourceUsername);
            if (follower == null)
                return NotFound();

            var following = await this._unitOfWork.Users.GetUser(targetUsername);
            if (following == null)
                return NotFound();

            var result = await this._userService.IsFollowed(follower.UserName, following.UserName);

            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string key)
        {
            var users = await this._userService.SearchUser(key);

            var result = this._mapper.Map<IList<AppUser>, IList<SearchUserResource>>(users);

            return Ok(result);
        }

        [HttpGet("profile")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserProfile(string sourceUsername, string targetUsername)
        {
            var sourceUser = await this._unitOfWork.Users.GetUser(sourceUsername);
            var user = await this._unitOfWork.Users.GetUserProfile(targetUsername);

            if (sourceUser == null || user == null)
                return BadRequest();

            var result = this._mapper.Map<AppUser, UserResource>(user);

            if (await this._userService.IsFollowed(sourceUsername, targetUsername))
                result.isFollowed = true;

            return Ok(result);
        }

        [HttpGet("sendChangeEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> SendChangeEmailRequest(string username, string newEmail)
        {
            var callbackurl = "http://localhost:5000/api/users/confirmChangeEmail";

            await this._userService.SendChangeEmailRequest(username, newEmail, callbackurl);

            return Ok();
        }

        [HttpGet("confirmChangeEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmChangeEmail(string username, string newEmail, string token)
        {
            var user = this._unitOfWork.Users.GetUser(username);
            if (user == null)
                return BadRequest();

            var result = await this._userService.ConfirmChangeEmail(username, newEmail, token);

            if (result)
                return Ok();

            return Problem();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest user)
        {
            var errors = await _userService.Register(user);
            var error = new
            {
                EmailError = "Email already in use",
                UserNameError = "Username already exists",
            };

            if (errors == null)
            {
                return Ok();
            }
            else
            {
                if (errors.Count == 1)
                {
                    if (errors[0] == "Email already in use")
                    {
                        error = new
                        {
                            EmailError = "Email already in use",
                            UserNameError = "",
                        };
                    }
                    else
                    {
                        error = new
                        {
                            EmailError = "",
                            UserNameError = "Username already exists",
                        };
                    }
                }
            }

            return BadRequest(error);
        }

        [HttpPut("updateProfile")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfieResource userProfileResource)
        {
            var user = await this._unitOfWork.Users.GetUser(userProfileResource.UserName);
            if (user == null)
                return BadRequest();

            this._mapper.Map<UpdateUserProfieResource, AppUser>(userProfileResource, user);
            await this._unitOfWork.Complete();

            return Ok();
        }

        [HttpPut("changePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordResource updatePasswordResource)
        {
            var user = await this._unitOfWork.Users.GetUser(updatePasswordResource.UserName);
            if (user == null)
                return BadRequest("User not found");

            if (updatePasswordResource.CurrentPassword == updatePasswordResource.NewPassword)
                ModelState.AddModelError("NewPasswordConfirm", "The new password and the old password cannot be the same");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await this._userService.ChangePassword(user.UserName, updatePasswordResource.CurrentPassword, updatePasswordResource.NewPassword);

            if (result == true)
                return Ok();

            return Problem();
        }
    }
}
