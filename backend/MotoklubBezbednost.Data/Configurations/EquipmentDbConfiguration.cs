using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Configurations;

public sealed class EquipmentDbConfiguration : IEntityTypeConfiguration<EquipmentDb>
{
    public void Configure(EntityTypeBuilder<EquipmentDb> entity)
    {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.MemberId);

        entity.HasOne<MemberDb>()
            .WithOne(m => m.Equipment)
            .HasForeignKey<EquipmentDb>(e => e.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

