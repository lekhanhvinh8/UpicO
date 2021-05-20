using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Controllers.Resources;
using Upico.Core;
using Upico.Core.Domain;
using Upico.Core.Services;

namespace Upico.Controllers
{
    [Route("api/posts")]
    //[Authorize]
    [ApiController]

    public class PostsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public PostsController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._photoService = photoService;
        }

        [HttpGet("user/{userName}")]
        public async Task<IActionResult> GetPosts(string userName)
        {
            var user = await this._unitOfWork.Users.GetUser(userName);
            if (user == null)
                return NotFound();

            //Load all posts of user
            await this._unitOfWork.Posts.Load(p => p.User.Id == user.Id);

            var posts = user.Posts;

            var result = this._mapper.Map<IList<Post>, IList<PostResouce>>(posts);

            return Ok(result);

        } 

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostDetail(string postId)
        {
            var post = await this._unitOfWork.Posts.SingleOrDefault(p => p.Id.ToString() == postId);
            if (post == null)
                return NotFound();

            await this._unitOfWork.PostedImages.Load(i => i.PostId == post.Id);

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreatePostResource postResource)
        {
            var user = await this._unitOfWork.Users.GetUser(postResource.UserName);
            if (user == null)
                return NotFound();

            var post = this._mapper.Map<CreatePostResource, Post>(postResource);
            post.DateCreate = DateTime.Now;

            user.Posts.Add(post);
            await this._unitOfWork.Complete();

            return Ok();
        }



    }
}
