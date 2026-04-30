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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}


