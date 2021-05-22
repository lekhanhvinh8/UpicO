using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upico.Controllers.Resources
{
    public class UserResource
    {
        public string Id { set; get; }
        public string UserName { get; set; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string FullName { set; get; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
    }
}
