using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core;
using Upico.Core.Domain;
using Upico.Core.Services;

namespace Upico.Persistence.Service
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<IList<Comment>> GetParentComments(string postId, int numComments)
        {

            //var post 



            throw new NotImplementedException();
        }
        public Task<IList<Comment>> GetMoreParentComments(string postId, string lastCommentId, int numComments)
        {
            throw new NotImplementedException();
        }
        

        public Task<IList<Comment>> GetChidrennComments(string parentId, int numComments)
        {
           





            throw new NotImplementedException();
        }

        public Task<IList<Comment>> GetMoreChidrennComments(string parentId, string lastCommentId, int numComments)
        {
            throw new NotImplementedException();
        }

        
    }
}
