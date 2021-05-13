using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Upico.Controllers.Resources;
using Upico.Core;

namespace Upico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            //this._mapper = mapper;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            var result = await _unitOfWork.Users.Authenticate(request);

            if (string.IsNullOrEmpty(result))
                return BadRequest("Tài khoản hoặc mật khẩu không chính xác");

            return Ok(result);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByName(string username)
        {
            var result = await _unitOfWork.Users.GetUser(username);

            if (result == null)
                return BadRequest("User không tồn tại");

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest user)
        {
            var result = await _unitOfWork.Users.Register(user);

            if (string.IsNullOrEmpty(result))
            {
                return Ok();
            }
            return BadRequest(result);
        }
    }
}
