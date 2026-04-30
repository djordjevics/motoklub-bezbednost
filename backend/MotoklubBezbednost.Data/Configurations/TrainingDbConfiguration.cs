using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Configurations;

public sealed class TrainingDbConfiguration : IEntityTypeConfiguration<TrainingDb>
{
    public void Configure(EntityTypeBuilder<TrainingDb> entity)
    {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.MemberId);
        entity.HasIndex(e => e.MotorcycleId);
        entity.HasIndex(e => e.TrainingSessionId);

        entity.HasOne<MemberDb>()
            .WithMany(m => m.Trainings)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Motorcycle)
            .WithMany(m => m.Trainings)
            .HasForeignKey(e => e.MotorcycleId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.TrainingSession)
            .WithMany(ts => ts.Trainings)
            .HasForeignKey(e => e.TrainingSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

