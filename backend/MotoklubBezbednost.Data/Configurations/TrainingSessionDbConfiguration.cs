using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Configurations;

public sealed class TrainingSessionDbConfiguration : IEntityTypeConfiguration<TrainingSessionDb>
{
    public void Configure(EntityTypeBuilder<TrainingSessionDb> entity)
    {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.LevelId);

        entity.HasOne(e => e.Level)
            .WithMany(l => l.TrainingSessions)
            .HasForeignKey(e => e.LevelId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

