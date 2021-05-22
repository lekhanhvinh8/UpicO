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

        [HttpPost("comment")]
        public async Task<IActionResult> Comment([FromBody] CreatedCommentResouce commentResouce)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.Users.GetUser(commentResouce.Username);
            if (user == null)
                return BadRequest();

            var post = await _unitOfWork.Posts.SingleOrDefault(p => p.Id.ToString() == commentResouce.PostId);
            if (post == null)
                return BadRequest();

            var comment = new Comment();
            comment.Post = post;
            comment.User = user;
            comment.Content = commentResouce.Content;
            comment.DateCreate = DateTime.Now;

            await this._unitOfWork.Comments.Add(comment);
            await this._unitOfWork.Complete();

            return Ok();
        }

        [HttpPost("reply")]
        public async Task<IActionResult> Reply([FromBody] ReplyResource replyResouce)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.Users.GetUser(replyResouce.Username);
            if (user == null)
                return BadRequest();

            var commentParent = await _unitOfWork.Comments.SingleOrDefault(c => c.Id.ToString() == replyResouce.ParentId);
            if (commentParent == null)
                return BadRequest();

            var comment = new Comment();
            comment.Parent = commentParent;
            comment.User = user;
            comment.Content = replyResouce.Content;
            comment.DateCreate = DateTime.Now;

            await this._unitOfWork.Comments.Add(comment);
            await this._unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteComment(string commentId)
        {
            var comment = await this._unitOfWork.Comments.SingleOrDefault(c => c.Id.ToString() == commentId);
            if (comment == null)
                return BadRequest();

            this._unitOfWork.Comments.RemoveAllChildren(commentId);
            this._unitOfWork.Comments.Remove(comment);
            await this._unitOfWork.Complete();

            return Ok();
        }
    }
}