using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Controllers.Resouces;

namespace Upico.Controllers.Resources
{
    public class DetailedPostResource
    {
        public Guid Id { set; get; }
        public string Username { set; get; }
        public string Content { set; get; }
        public DateTime DateCreate { set; get; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
        public bool PrivateMode { get; set; }
        public bool IsLiked { get; set; }
        public IList<PhotoResource> PostImages { set; get; }
        public int Likes { set; get; }
        public IList<string> Comments { set; get; }
        public DetailedPostResource()
        {
            PostImages = new List<PhotoResource>();
            Comments = new List<string>();
        }
    }
}
