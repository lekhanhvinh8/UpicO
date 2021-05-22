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
            //Load another info
            this._context.Users.Where(u => u.Id == comment.UserId).Load();
            this._context.Avatars.Where(a => a.IsMain && a.UserID == comment.UserId).Load();


            //Load childs info

            this._context.Comments.Where(c => c.ParentId == comment.Id).Load();

            if (comment.Childs.Count == 0)
                return;

            foreach (var child in comment.Childs)
            {
                LoadAllChildren(child);
            }
        }

        public void RemoveAllChildren(string commentId)
        {
            var comment = this._context.Comments.SingleOrDefault(c => c.Id.ToString() == commentId);

            //Load childs info
            LoadAllChildren(comment);

            RemoveAllChildren(comment);
        }

        private void RemoveAllChildren(Comment comment)
        {
            var removeComments = new List<Comment>();

            foreach (var child in comment.Childs)
            {
                if (child.Childs.Count == 0)
                    removeComments.Add(child);
                else
                    RemoveAllChildren(child);
            }

            comment.Childs.Clear();
            this._context.Comments.RemoveRange(removeComments);
        }
    }
}
