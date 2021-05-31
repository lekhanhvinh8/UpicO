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
using Upico.Core.Services;

namespace Upico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUserService userService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
        

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest user)
        {
            var errors = await _userService.Register(user);
            var error = new {
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

        [HttpGet("{sourceUsername}/{targetUsername}")]
        [AllowAnonymous]
        public async Task<IActionResult> Follow(string sourceUsername, string targetUsername)
        {
            var follower = await this._unitOfWork.Users.GetUser(sourceUsername);
            if (follower == null)
                return NotFound();

            var following = await this._unitOfWork.Users.GetUser(targetUsername);
            if (following == null)
                return NotFound();

            follower.Followings.Add(following);
            await this._unitOfWork.Complete();

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string key)
        {
            var users = await this._userService.SearchUser(key);

            var result = this._mapper.Map<IList<AppUser>, IList<SearchUserResource>>(users);

            return Ok(result);
        }
    }
}
