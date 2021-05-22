using Upico.Core.Domain;

namespace Upico.Core.Repositories
{
    public interface ICommentRepository:IRepository<Comment>
    {
        public void LoadAllChildren(Comment comment);
    }
}
