using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upico.Controllers.Resources
{
    public class AvatarResource
    {
        public Guid Id { get; set; }
        public string Path { get; set; }

        public string UserID { get; set; }

        public int IsMain { get; set; }
    }
}
