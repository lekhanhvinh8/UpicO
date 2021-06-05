using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core;
using Upico.Core.Domain;
using Upico.Core.Services;
using Upico.Core.StaticValues;

namespace Upico.Controllers
{
    [Authorize(Roles =RoleNames.RoleAdmin)]
    [Route("/api/admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUserService userService, IUnitOfWork unitOfWork)
        {
            this._userService = userService;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportedPost()
        {
            var reporteds = await this._unitOfWork.ReportedPosts.GetAll();

            reporteds.FirstOrDefault();

            return Ok(reporteds);
        }

    }
}
