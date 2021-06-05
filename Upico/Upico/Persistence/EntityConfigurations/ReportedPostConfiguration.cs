using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core.Domain;

namespace Upico.Persistence.EntityConfigurations
{
    public class ReportedPostConfiguration : IEntityTypeConfiguration<ReportedPost>
    {
        public void Configure(EntityTypeBuilder<ReportedPost> builder)
        {
            builder.HasKey(r => new {r.PostId, r.ReporterId});

            builder.HasOne(r => r.Post)
                .WithMany(p => p.ReportedPosts)
                .HasForeignKey(r => r.PostId);

            builder.HasOne(r => r.Reporter)
                .WithMany(u => u.ReportedPosts)
                .HasForeignKey(r => r.ReporterId);
        }
    }
}
