using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Configurations;

public sealed class MotorcycleDbConfiguration : IEntityTypeConfiguration<MotorcycleDb>
{
    public void Configure(EntityTypeBuilder<MotorcycleDb> entity)
    {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.MemberId);

        entity.HasOne<MemberDb>()
            .WithMany(m => m.Motorcycles)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

