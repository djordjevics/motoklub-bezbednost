using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Configurations;

public sealed class TagDbConfiguration : IEntityTypeConfiguration<TagDb>
{
    public void Configure(EntityTypeBuilder<TagDb> entity)
    {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.MemberId);

        entity.HasOne<MemberDb>()
            .WithMany(m => m.Tags)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

