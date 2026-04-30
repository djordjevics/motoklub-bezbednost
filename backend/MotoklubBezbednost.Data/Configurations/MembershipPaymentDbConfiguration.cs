using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Configurations;

public sealed class MembershipPaymentDbConfiguration : IEntityTypeConfiguration<MembershipPaymentDb>
{
    public void Configure(EntityTypeBuilder<MembershipPaymentDb> entity)
    {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.MemberId);
        entity.HasIndex(e => e.PaymentTypeId);

        entity.HasOne<MemberDb>()
            .WithMany(m => m.MembershipPayments)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.PaymentType)
            .WithMany(pt => pt.MembershipPayments)
            .HasForeignKey(e => e.PaymentTypeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

