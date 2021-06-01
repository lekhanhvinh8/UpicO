using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upico.Controllers.Resources
{
    public class UpdateUserProfieResource
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string FullName { set; get; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
    }
}
