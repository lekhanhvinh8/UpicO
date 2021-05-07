using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core.Domain;

namespace Upico.Persistence.EntityConfigurations
{
    public class LikeConfigurations : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(a => new { a.PostId, a.UserId });

            builder.HasOne(a => a.User)
                .WithMany(au => au.Likes)
                .HasForeignKey(a => a.UserId);

            builder.HasOne(a => a.Post)
                .WithMany(au => au.Likes)
                .HasForeignKey(a => a.PostId);

        }
    }
}
