using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core.Domain;

namespace Upico.Core.Services
{
    public interface ICommentService
    {
        public Task<IList<Comment>> GetParentComments(string postId, int numComments);
        public Task<IList<Comment>> GetMoreParentComments(string postId, string lastCommentId, int numComments);
        public Task<IList<Comment>> GetChidrennComments(string parentId, int numComments);
        public Task<IList<Comment>> GetMoreChidrennComments(string parentId, string lastCommentId, int numComments);



    }
}
