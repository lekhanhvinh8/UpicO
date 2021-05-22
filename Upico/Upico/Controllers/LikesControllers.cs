﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core;

namespace Upico.Controllers
{
    [Route("api/likes")]
    //[Authorize]
    [ApiController]
    public class LikesControllers : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LikesControllers(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Like(string username, string postId)
        {

            var user = await this._unitOfWork.Users.GetUserWithLikes(username);
            var post = await this._unitOfWork.Posts.SingleOrDefault(p => p.Id.ToString() == postId);

            if (user == null || post == null)
                return NotFound();

            if (user.Likes.Contains(post))
                return BadRequest("User has already liked this post");

            user.Likes.Add(post);
            await this._unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Dislike(string username, string postId)
        {
            var user = await this._unitOfWork.Users.GetUserWithLikes(username);
            var post = await this._unitOfWork.Posts.SingleOrDefault(p => p.Id.ToString() == postId);

            if (user == null || post == null)
                return NotFound();

            if (!user.Likes.Contains(post))
                return BadRequest("user has not liked this post yet");

            user.Likes.Remove(post);
            await this._unitOfWork.Complete();

            return Ok();
        }

    }
}
