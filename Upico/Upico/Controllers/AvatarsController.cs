using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Controllers.Resources;
using Upico.Core;
using Upico.Core.Domain;
using Upico.Persistence;

namespace Upico.Controllers
{
    [Route("/api/avatars")]
    public class AvatarsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AvatarsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> Get(string userName)
        {
            var user = await this._unitOfWork.Users.GetUser(userName);
            if (user == null)
                return BadRequest();

            var avatars = await this._unitOfWork.Avatars.GetAvatar(userName);

            await this._unitOfWork.Complete();

            var result = this._mapper.Map<List<Avatar>, List<AvatarResource>>(avatars);

            return Ok(result);
        } 
        
    }
}
