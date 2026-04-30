using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Configurations;

public sealed class LevelDbConfiguration : IEntityTypeConfiguration<LevelDb>
{
    public void Configure(EntityTypeBuilder<LevelDb> entity)
    {
        entity.HasKey(e => e.Id);
    }
}

