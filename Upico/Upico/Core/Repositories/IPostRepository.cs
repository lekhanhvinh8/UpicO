using System.Collections.Generic;
using System.Threading.Tasks;
using Upico.Core.Domain;

namespace Upico.Core.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        public Task<Post> GetPostDetail(string postId);
        public Task<IList<Post>> GetRelatedPosts(string userName, int numPost);
        public Task<IList<Post>> GetRelatedPostsBefore(string userName, string latestPostId, int numPosts);
    }
}
