using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<MemberDb> Members { get; set; }
    public DbSet<MemberTypeDb> MemberTypes { get; set; }
    public DbSet<MembershipPaymentDb> MembershipPayments { get; set; }
    public DbSet<PaymentTypeDb> PaymentTypes { get; set; }
    public DbSet<MotorcycleDb> Motorcycles { get; set; }
    public DbSet<EquipmentDb> Equipment { get; set; }
    public DbSet<TrainingSessionDb> TrainingSessions { get; set; }
    public DbSet<LevelDb> Levels { get; set; }
    public DbSet<TrainingDb> Trainings { get; set; }
    public DbSet<CommentDb> Comments { get; set; }
    public DbSet<TagDb> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Member configuration
        modelBuilder.Entity<MemberDb>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Jmbg).IsUnique();
            entity.HasIndex(e => e.Email);
            entity.HasOne(e => e.MemberType)
                  .WithMany(mt => mt.Members)
                  .HasForeignKey("MemberTypeId")
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // MemberType configuration
        modelBuilder.Entity<MemberTypeDb>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // MembershipPayment configuration
        modelBuilder.Entity<MembershipPaymentDb>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
                  .WithMany(m => m.MembershipPayments)
                  .HasForeignKey("MemberId")
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.PaymentType)
                  .WithMany(pt => pt.MembershipPayments)
                  .HasForeignKey("PaymentTypeId")
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // PaymentType configuration
        modelBuilder.Entity<PaymentTypeDb>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // Motorcycle configuration
        modelBuilder.Entity<MotorcycleDb>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
                  .WithMany(m => m.Motorcycles)
                  .HasForeignKey("MemberId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Equipment configuration (One-to-One with Member)
        modelBuilder.Entity<EquipmentDb>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
                  .WithOne(m => m.Equipment)
                  .HasForeignKey<EquipmentDb>("MemberId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // TrainingSession configuration
        modelBuilder.Entity<TrainingSessionDb>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Level)
                  .WithMany(l => l.TrainingSessions)
                  .HasForeignKey("LevelId")
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Level configuration
        modelBuilder.Entity<LevelDb>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // Training configuration
        modelBuilder.Entity<TrainingDb>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
                  .WithMany(m => m.Trainings)
                  .HasForeignKey("MemberId")
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Motorcycle)
                  .WithMany(m => m.Trainings)
                  .HasForeignKey("MotorcycleId")
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.TrainingSession)
                  .WithMany(ts => ts.Trainings)
                  .HasForeignKey("TrainingSessionId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Comment configuration
        modelBuilder.Entity<CommentDb>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
                  .WithMany(m => m.Comments)
                  .HasForeignKey("MemberId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Tag configuration
        modelBuilder.Entity<TagDb>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
                  .WithMany(m => m.Tags)
                  .HasForeignKey("MemberId")
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}


