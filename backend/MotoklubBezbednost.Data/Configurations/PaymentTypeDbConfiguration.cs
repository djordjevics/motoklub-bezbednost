using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Configurations;

public sealed class PaymentTypeDbConfiguration : IEntityTypeConfiguration<PaymentTypeDb>
{
    public void Configure(EntityTypeBuilder<PaymentTypeDb> entity)
    {
        entity.HasKey(e => e.Id);
    }
}

