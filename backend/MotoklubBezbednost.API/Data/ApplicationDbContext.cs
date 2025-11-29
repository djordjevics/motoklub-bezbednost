using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<MemberType> MemberTypes { get; set; }
    public DbSet<MembershipPayment> MembershipPayments { get; set; }
    public DbSet<PaymentType> PaymentTypes { get; set; }
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<TrainingSession> TrainingSessions { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<Training> Trainings { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Member configuration
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Jmbg).IsUnique();
            entity.HasIndex(e => e.Email);
            entity.HasOne(e => e.MemberType)
                  .WithMany(mt => mt.Members)
                  .HasForeignKey(e => e.MemberTypeId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // MemberType configuration
        modelBuilder.Entity<MemberType>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // MembershipPayment configuration
        modelBuilder.Entity<MembershipPayment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
                  .WithMany(m => m.MembershipPayments)
                  .HasForeignKey(e => e.MemberId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.PaymentType)
                  .WithMany(pt => pt.MembershipPayments)
                  .HasForeignKey(e => e.PaymentTypeId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // PaymentType configuration
        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // Motorcycle configuration
        modelBuilder.Entity<Motorcycle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
                  .WithMany(m => m.Motorcycles)
                  .HasForeignKey(e => e.MemberId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Equipment configuration (One-to-One with Member)
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
                  .WithOne(m => m.Equipment)
                  .HasForeignKey<Equipment>(e => e.MemberId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // TrainingSession configuration
        modelBuilder.Entity<TrainingSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Level)
                  .WithMany(l => l.TrainingSessions)
                  .HasForeignKey(e => e.LevelId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Level configuration
        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // Training configuration
        modelBuilder.Entity<Training>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
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
        });

        // Comment configuration
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
                  .WithMany(m => m.Comments)
                  .HasForeignKey(e => e.MemberId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Tag configuration
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
                  .WithMany(m => m.Tags)
                  .HasForeignKey(e => e.MemberId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

