using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upico.Core.Domain
{
    public class Avatar
    {
        public Guid Id { get; set; }
        public string Path { get; set; }

        public string UserID { get; set; }
        public AppUser AppUser { get; set; }

        public int IsMain { get; set; }
    }
}
