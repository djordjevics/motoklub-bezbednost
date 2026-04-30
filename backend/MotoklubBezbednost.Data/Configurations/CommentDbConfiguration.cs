using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Configurations;

public sealed class CommentDbConfiguration : IEntityTypeConfiguration<CommentDb>
{
    public void Configure(EntityTypeBuilder<CommentDb> entity)
    {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.MemberId);

        entity.HasOne<MemberDb>()
            .WithMany(m => m.Comments)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

