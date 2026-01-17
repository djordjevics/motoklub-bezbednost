using MotoklubBezbednost.API.Mappers;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.API.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all mapper services (Db <-> Dto mappers).
    /// </summary>
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        // Register Db <-> Dto mappers (only one direction needed - Mapper facade handles reverse)
        services.AddScoped<ITwoWayMapper<MemberDb, MemberDto>, MemberMapper>();
        services.AddScoped<ITwoWayMapper<EquipmentDb, EquipmentDto>, EquipmentMapper>();
        services.AddScoped<ITwoWayMapper<MotorcycleDb, MotorcycleDto>, MotorcycleMapper>();
        services.AddScoped<ITwoWayMapper<TrainingSessionDb, TrainingSessionDto>, TrainingSessionMapper>();
        services.AddScoped<ITwoWayMapper<TrainingDb, TrainingDto>, TrainingMapper>();

        // Register central mapper facade
        services.AddScoped<IMapper, Mapper>();

        return services;
    }
}

