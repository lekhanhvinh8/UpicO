using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Upico.Controllers.Resources;
using Upico.Core;
using Upico.Core.Services;

namespace Upico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUserService userService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
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

        /*
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByName(string username)
        {
            var result = await _userService.GetUser(username);

            if (result == null)
                return BadRequest("User không tồn tại");

            var user = User;
            var claimName = user.FindFirst(ClaimTypes.Name);

            if (claimName.Value != username)
                return Unauthorized();

            return Ok(result);
        }
        */

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest user)
        {
            var errors = await _userService.Register(user);
            var error = new {
                EmaiError = "Email already in use",
                UserNameError = "Username is existed",
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
                            EmaiError = "Email already in use",
                            UserNameError = "",
                        };
                    }
                    else
                    {
                        error = new
                        {
                            EmaiError = "",
                            UserNameError = "Username is existed",
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
    }
}
