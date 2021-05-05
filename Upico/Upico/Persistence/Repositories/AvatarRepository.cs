using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core.Domain;
using Upico.Core.Repositories;

namespace Upico.Persistence.Repositories
{
    public class AvatarRepository : Repository<Avatar>, IAvatarRepository
    {

        public AvatarRepository(UpicODbContext context)
            :base(context)
        {
        }
        public async Task<Avatar> DoSomething()
        {
            // Add new avatar to user vinh 07

            var user = this.Context.Set<AppUser>().SingleOrDefault(u => u.UserName == "vinh");

            var avatar = new Avatar();
            avatar.Path = "vinhAvatar.png";

            user.Avatars.Add(avatar);

            await this.Context.SaveChangesAsync();

            return avatar;

        }
    }
}
