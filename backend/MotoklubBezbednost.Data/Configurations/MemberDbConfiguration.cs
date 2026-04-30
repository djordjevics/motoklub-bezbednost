using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Configurations;

public sealed class MemberDbConfiguration : IEntityTypeConfiguration<MemberDb>
{
    public void Configure(EntityTypeBuilder<MemberDb> entity)
    {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.Jmbg).IsUnique();
        entity.HasIndex(e => e.Email);

        entity.HasOne(e => e.MemberType)
            .WithMany(mt => mt.Members)
            .HasForeignKey(e => e.MemberTypeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

