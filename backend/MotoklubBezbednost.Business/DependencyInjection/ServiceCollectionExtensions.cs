using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotoklubBezbednost.Data;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMotoklubPersistence(this IServiceCollection services, string sqliteConnectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(sqliteConnectionString, sqlite =>
                sqlite.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)));

        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<ITrainingRepository, TrainingRepository>();
        services.AddScoped<ITrainingSessionRepository, TrainingSessionRepository>();
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();

        return services;
    }

    public static void ApplyMotoklubMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
}
