﻿using AutoMapper;
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
        private readonly IUserService _userService;

        public PostsController(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._userService = userService;
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

        [HttpGet("user/{userName}/{numPosts}")]
        public async Task<IActionResult> GetRelatedPosts(string userName, int numPosts)
        {
            var user = await this._unitOfWork.Users.GetUser(userName);
            if (user == null)
                return NotFound();

            var posts = await this._unitOfWork.Posts.GetRelatedPosts(userName, numPosts);

            var result = this._mapper.Map<IList<Post>, IList<DetailedPostResource>>(posts);

            return Ok(result);
        }

        [HttpGet("user/{userName}/{latestPostId}/{numPosts}")]
        public async Task<IActionResult> GetRelatedPostsBefore(string userName, string latestPostId, int numPosts)
        {
            var user = await this._unitOfWork.Users.GetUser(userName);
            if (user == null)
                return NotFound();

            var latestPost = await this._unitOfWork.Posts.SingleOrDefault(p => p.Id.ToString() == latestPostId);
            if (latestPost == null)
                return NotFound();

            var posts = await this._unitOfWork.Posts.GetRelatedPostsBefore(userName, latestPostId, numPosts);

            var result = this._mapper.Map<IList<Post>, IList<DetailedPostResource>>(posts);

            return Ok(result);
        }

        [HttpGet("UserProfilePosts")]
        public async Task<IActionResult> getPosts(string sourceUsername, string targetUsername, int numPosts)
        {
            var sourceUser = await this._unitOfWork.Users.GetUser(sourceUsername);
            var targetUser = await this._unitOfWork.Users.GetUser(targetUsername);

            if (sourceUser == null || targetUser == null)
                return BadRequest();

            var getPrivatePost = await this._userService.IsFollowed(sourceUsername, targetUsername);

            IList<Post> posts = await this._unitOfWork.Posts.GetPosts(targetUsername, getPrivatePost, numPosts);
            posts = posts.Where(p => p.PostImages.Count > 0).ToList();

            var result = this._mapper.Map<IList<Post>, IList<PostUserProfileResource>>(posts);

            return Ok(result);
        }

        [HttpGet("MoreUserProfilePosts")]
        public async Task<IActionResult> getPostsBefore(string sourceUsername, string targetUsername, string latestPostId, int numPosts)
        {
            var sourceUser = await this._unitOfWork.Users.GetUser(sourceUsername);
            var targetUser = await this._unitOfWork.Users.GetUser(targetUsername);

            if (sourceUser == null || targetUser == null)
                return BadRequest();

            var latestPost = await this._unitOfWork.Posts.SingleOrDefault(p => p.Id.ToString() == latestPostId);
            if (latestPost == null)
                return NotFound();

            var getPrivatePost = await this._userService.IsFollowed(sourceUsername, targetUsername);

            IList<Post> posts = await this._unitOfWork.Posts.GetPostsBefore(targetUsername, latestPostId, getPrivatePost, numPosts);
            posts = posts.Where(p => p.PostImages.Count > 0).ToList();

            var result = this._mapper.Map<IList<Post>, IList<PostUserProfileResource>>(posts);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPostDetail(string postId)
        {
            var post = await this._unitOfWork.Posts.GetPostDetail(postId);
            if (post == null)
                return NotFound();

            var result = this._mapper.Map<Post, DetailedPostResource>(post);

            return Ok(result);
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

            var detailedPost = await this._unitOfWork.Posts.GetPostDetail(post.Id.ToString());

            var result = this._mapper.Map<Post, DetailedPostResource>(detailedPost);
            return Ok(result);
        }

    }
}
