using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upico.Core.Domain
{
    public class Like
    {
        public string UserId { set; get; }
        public Guid PostId { set; get; }
        public AppUser User { set; get; }
        public Post Post { set; get; }
    }
}
