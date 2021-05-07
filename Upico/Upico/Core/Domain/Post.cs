using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upico.Core.Domain
{
    public class Post
    {
        public Guid Id { set; get; }
        public string UserId { set; get; }
        public string? Content { set; get; }
        public DateTime DateCreate { set; get; }

        public AppUser User { set; get; }
        public IList<PostImage> PostImages { set; get; }
        public IList<Like> Likes { set; get; }
        public IList<Comment> Comments { set; get; }
        public Post()
        {
            PostImages = new List<PostImage>();
            Likes = new List<Like>();
            Comments = new List<Comment>();
        }
    }
}
