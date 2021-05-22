using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Controllers.Resources;
using Upico.Core;
using Upico.Core.Domain;

namespace Upico.Controllers
{
    [Route("api/comments")]
    //[Authorize]
    [ApiController]
    public class CommentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string postId)
        {
            var post = await this._unitOfWork.Posts.SingleOrDefault(p => p.Id.ToString() == postId);

            await this._unitOfWork.Comments.Load(c => c.PostId == post.Id);

            foreach (var comment in post.Comments)
            {
                this._unitOfWork.Comments.LoadAllChildren(comment);
            }

            var result = this._mapper.Map<IList<Comment>, IList<CommentResouce>>(post.Comments);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Comment([FromBody] SaveCommentResouce commentResouce)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.Users.GetUser(commentResouce.UserName);
            if (user == null)
                return BadRequest();

            var post = await _unitOfWork.Posts.SingleOrDefault(p => p.Id.ToString() == commentResouce.PostId);
            if (post == null)
                return BadRequest();

            var comment = new Comment();
            comment.Post = post;
            comment.User = user;
            comment.DateCreate = DateTime.Now;

            await this._unitOfWork.Comments.Add(comment);

            return Ok();
        }

    }
}
