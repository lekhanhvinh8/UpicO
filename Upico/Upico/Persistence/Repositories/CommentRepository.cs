using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core.Domain;
using Upico.Core.Repositories;

namespace Upico.Persistence.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly UpicODbContext _context;

        public CommentRepository(UpicODbContext context)
            :base(context)
        {
            _context = context;
        }

        public void LoadAllChildren(Comment comment)
        {
            this._context.Comments.Where(c => c.ParentId == comment.Id).Load();

            if (comment.Childs.Count == 0)
                return;

            foreach (var child in comment.Childs)
            {
                LoadAllChildren(child);
            }
        }
    }
}
