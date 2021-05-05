using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Upico.Core.Domain;
using Upico.Persistence.EntityConfigurations;

namespace Upico.Persistence
{
    public class UpicODbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Avatar> Avatars { get; set; }
        public UpicODbContext(DbContextOptions<UpicODbContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AvatarConfigurations());
        }
    }
}
