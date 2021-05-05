using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core;
using Upico.Core.Domain;
using Upico.Persistence;

namespace Upico.Controllers
{
    [Route("/api/avatars")]
    public class AvatarsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AvatarsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<Avatar> Get()
        {
            var avatar = await this._unitOfWork.Avatars.DoSomething();
            return null;
        } 
    }
}
