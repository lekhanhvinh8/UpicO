using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core.Domain;
using Upico.Core.Repositories;

namespace Upico.Persistence.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly UpicODbContext _context;

        public PostRepository(UpicODbContext context)
            :base(context)
        {
            _context = context;
        }

        public async Task<Post> GetPostDetail(string postId)
        {
            var post = await this._context.Posts.Include(p => p.Likes)
                                                .Include(p => p.Comments.Where(c => c.Parent == null).Take(3))
                                                .Include(p => p.PostImages)
                                                .Include(p => p.User)
                                                .Include(p => p.User.Avatars.Where(a => a.IsMain))
                                                .SingleOrDefaultAsync(p => p.Id.ToString() == postId);

            return post;
        }

        private async Task<IQueryable<Post>> GetRelatedPosts(string userName)
        {
            var user = await this._context.Users
                .Include(u => u.Avatars.Where(a => a.IsMain))
                .Include(u => u.Followings).ThenInclude(f => f.Avatars.Where(a => a.IsMain))
                .SingleOrDefaultAsync(u => u.UserName == userName);

            var relatedPost =  this._context.Posts.Where(p => p.UserId == user.Id || user.Followings.Select(u => u.Id).Contains(p.UserId));

            return relatedPost;
        }

        public async Task<IList<Post>> GetRelatedPosts(string userName, int numPosts)
        {
            var posts = await GetRelatedPosts(userName);

            var newPosts = await posts.OrderByDescending(p => p.DateCreate)
                .Include(p => p.Likes)
                .Include(p => p.Comments.Where(c => c.Parent == null).Take(3))
                .Include(p => p.PostImages)
                .Take(numPosts)
                .ToListAsync();

            return newPosts;
        }

        public async Task<IList<Post>> GetRelatedPostsBefore(string userName, string latestPostId, int numPosts)
        {
            var latestPost = await this._context.Posts.SingleOrDefaultAsync(p => p.Id.ToString() == latestPostId);

            var posts = await GetRelatedPosts(userName);
            
            var newPosts = await posts.Where(p => p.DateCreate < latestPost.DateCreate)
                .OrderByDescending(p => p.DateCreate)
                .Include(p => p.Likes)
                .Include(p => p.Comments.Where(c => c.Parent == null).Take(3))
                .Include(p => p.PostImages)
                .Take(numPosts)
                .ToListAsync();

            return newPosts;
        }

        public async Task<IList<Post>> GetPosts(string username, bool getPrivatePost, int numPosts)
        {
            var user = await this._context.Users
                .Include(u => u.Avatars.Where(a => a.IsMain))
                .SingleOrDefaultAsync(u => u.UserName == username);

            var posts = await this._context.Posts
                .Include(p => p.Likes)
                .Include(p => p.Comments.Where(c => c.Parent == null).Take(3))
                .Include(p => p.PostImages)
                .Where(p => p.UserId == user.Id)
                .OrderByDescending(p => p.DateCreate)
                .Take(numPosts)
                .ToListAsync();

            if (!getPrivatePost)
                posts = posts.Where(p => p.PrivateMode == false).ToList();


            return posts;
        }

        public async Task<IList<Post>> GetPostsBefore(string username, string latestPostId, bool getPrivatePost, int numPosts)
        {
            var user = await this._context.Users
                .Include(u => u.Avatars.Where(a => a.IsMain))
                .SingleOrDefaultAsync(u => u.UserName == username);

            var latestPost = await this._context.Posts.SingleOrDefaultAsync(p => p.Id.ToString() == latestPostId);

            var posts = await this._context.Posts
                .Include(p => p.Likes)
                .Include(p => p.Comments.Where(c => c.Parent == null).Take(3))
                .Include(p => p.PostImages)
                .Include(p => p.User).ThenInclude(u => u.Avatars)
                .Where(p => p.UserId == user.Id && p.DateCreate < latestPost.DateCreate)
                .OrderByDescending(p => p.DateCreate)
                .Take(numPosts)
                .ToListAsync();

            if (!getPrivatePost)
                posts = posts.Where(p => p.PrivateMode == false).ToList();

            return posts;
        }
    }
}
