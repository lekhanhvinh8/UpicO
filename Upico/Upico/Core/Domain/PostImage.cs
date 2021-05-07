using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upico.Core.Domain
{
    public class PostImage
    {
        public Guid Id { set; get; }
        public Guid PostId { set; get; }
        public string Path { set; get; }
        public DateTime DateCreate { set; get; }

        public Post Post { set; get; }
    }
}
