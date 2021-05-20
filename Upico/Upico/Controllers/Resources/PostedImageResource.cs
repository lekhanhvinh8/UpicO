using System;

namespace Upico.Controllers.Resources
{

    public class PostedImageResource 
    {
        public Guid Id { set; get; }
        public Guid PostId { set; get; }
        public string Path { set; get; }
        public DateTime DateCreate { set; get; }
    }
}
