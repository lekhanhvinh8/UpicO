using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Controllers.Resources;
using Upico.Core;
using Upico.Core.Domain;
using Upico.Core.Services;
using Upico.Core.StaticValues;

namespace Upico.Controllers
{
    //[Authorize(Roles =RoleNames.RoleAdmin)]
    [Route("/api/admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminController(IUserService userService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._userService = userService;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportedPost()
        {
            var reportedPosts = await this._unitOfWork.ReportedPosts.GetAll();

            var postIds = reportedPosts.Select(r => r.PostId).Distinct().ToList();

            var firstReports = new List<FirstReportedPostResource>();

            foreach (var postId in postIds)
            {
                var reportedPostsWithId = reportedPosts
                    .Where(r => r.PostId == postId)
                    .OrderBy(r => r.DateCreated);

                var numOfReport = reportedPostsWithId.Count();
                var firstReport = reportedPostsWithId.FirstOrDefault();

                var user = await this._unitOfWork.Users.SearchUserById(firstReport.ReporterId);

                var reportResource = new FirstReportedPostResource()
                {
                    PostId = firstReport.PostId,
                    NumOfReports = numOfReport,
                    FirstReportTime = firstReport.DateCreated,
                    FirtsReporter = firstReport.Reporter.UserName,
                };

                firstReports.Add(reportResource);
            }
            return Ok(firstReports);
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetReportedPostDetail(string postId)
        {
            var post = await this._unitOfWork.Posts.GetReportedPost(postId);

            if (post == null)
                return BadRequest();

            var reports = this._unitOfWork.ReportedPosts.Find(r => r.PostId.ToString() == postId);
            if (reports.Count() == 0)
                return BadRequest();



            return Ok();
        }
    }
}
