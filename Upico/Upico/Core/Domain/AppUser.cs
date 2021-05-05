using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Upico.Core.Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public IList<Avatar> Avatars { get; set; }
        public AppUser()
        {
            this.Avatars = new List<Avatar>();
        }
    }
}
