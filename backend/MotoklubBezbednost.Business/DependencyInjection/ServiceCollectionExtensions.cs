using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotoklubBezbednost.Data;
using MotoklubBezbednost.Data.Exceptions;
using MotoklubBezbednost.Data.Interceptors;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMotoklubPersistence(this IServiceCollection services, string sqliteConnectionString)
    {
        services.AddSingleton<DbModelTimestampInterceptor>();

        // One ApplicationDbContext per request (scoped); standard for ASP.NET Core + EF Core.
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options
                .AddInterceptors(sp.GetRequiredService<DbModelTimestampInterceptor>())
                .UseSqlite(sqliteConnectionString, sqlite =>
                    sqlite.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));
        });

        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<ITrainingRepository, TrainingRepository>();
        services.AddScoped<ITrainingSessionRepository, TrainingSessionRepository>();
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<IMemberTypeRepository, MemberTypeRepository>();
        services.AddScoped<IMembershipPaymentRepository, MembershipPaymentRepository>();
        services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
        services.AddScoped<ILevelRepository, LevelRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddSingleton<IDataExceptionTranslator, SqliteDataExceptionTranslator>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static void ApplyMotoklubMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
}
