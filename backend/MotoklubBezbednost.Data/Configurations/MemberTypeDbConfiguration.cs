using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Configurations;

public sealed class MemberTypeDbConfiguration : IEntityTypeConfiguration<MemberTypeDb>
{
    public void Configure(EntityTypeBuilder<MemberTypeDb> entity)
    {
        entity.HasKey(e => e.Id);
    }
}

