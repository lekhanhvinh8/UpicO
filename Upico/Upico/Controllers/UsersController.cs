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

        public UsersController(IUserService userService)
        {
            _userService = userService;
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
                return BadRequest("User không tồn tại");

            var user = User;
            var claimName = user.FindFirst(ClaimTypes.Name);

            if (claimName.Value != username)
                return Unauthorized();

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest user)
        {
            var result = await _userService.Register(user);

            if (string.IsNullOrEmpty(result))
            {
                return Ok();
            }
            return BadRequest(result);
        }
    }
}
