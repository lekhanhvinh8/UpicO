using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core.Domain;

namespace Upico.Persistence.EntityConfigurations
{
    public class CommentConfigurations : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.User).WithMany(i => i.Comments).HasForeignKey(a => a.UserId);
            builder.HasOne(a => a.Post).WithMany(i => i.Comments).HasForeignKey(a => a.PostId);
            //builder.HasOne(a => a.Parent).WithMany().HasForeignKey(a => a.ParentId)
            //    .OnDelete(DeleteBehavior.Restrict)
            //    .IsRequired(false);

            builder.Property(a => a.Content).IsRequired();
        }
    }
}
