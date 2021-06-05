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
        public string Content { set; get; }
        public DateTime DateCreate { set; get; }
        public bool PrivateMode { get; set; }
        public AppUser User { set; get; }
        public IList<PostedImage> PostImages { set; get; }
        public IList<AppUser> Likes { set; get; }
        public IList<Comment> Comments { set; get; }
        public IList<ReportedPost> ReportedPosts { get; set; }
        public Post()
        {
            PostImages = new List<PostedImage>();
            Likes = new List<AppUser>();
            Comments = new List<Comment>();
            ReportedPosts = new List<ReportedPost>();
        }
    }
}
