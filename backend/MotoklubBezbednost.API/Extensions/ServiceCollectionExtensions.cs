using MotoklubBezbednost.API.Mappers;
using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Cqrs.Members.Queries;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.API.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all mapper services (Db <-> Dto mappers and Request -> Command/Query mappers).
    /// </summary>
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        // Register Db <-> Dto mappers (these also implement ITwoWayMapper and IOneWayMapper via inheritance)
        services.AddScoped<ITwoWayDbMapper<MemberDb, MemberDto>, MemberMapper>();
        services.AddScoped<ITwoWayDbMapper<EquipmentDb, EquipmentDto>, EquipmentMapper>();
        services.AddScoped<ITwoWayDbMapper<MotorcycleDb, MotorcycleDto>, MotorcycleMapper>();
        services.AddScoped<ITwoWayDbMapper<TrainingSessionDb, TrainingSessionDto>, TrainingSessionMapper>();
        services.AddScoped<ITwoWayDbMapper<TrainingDb, TrainingDto>, TrainingMapper>();

        // Register request mappers (one instance per mapper class that implements multiple IOneWayMapper interfaces)
        RegisterMemberRequestMappers(services);
        RegisterEquipmentRequestMappers(services);
        RegisterMotorcycleRequestMappers(services);
        RegisterTrainingRequestMappers(services);

        // Register central mapper facade
        services.AddScoped<IMapper, Mapper>();

        return services;
    }

    private static void RegisterMemberRequestMappers(IServiceCollection services)
    {
        services.AddScoped<MemberRequestMapper>();
        services.AddScoped<IOneWayMapper<GetAllMembersRequest, GetAllMembersQuery>>(sp => sp.GetRequiredService<MemberRequestMapper>());
        services.AddScoped<IOneWayMapper<CreateMemberRequest, CreateMemberCommand>>(sp => sp.GetRequiredService<MemberRequestMapper>());
        services.AddScoped<IOneWayMapper<UpdateMemberRequest, UpdateMemberCommand>>(sp => sp.GetRequiredService<MemberRequestMapper>());
        services.AddScoped<IOneWayMapper<DeleteMemberRequest, DeleteMemberCommand>>(sp => sp.GetRequiredService<MemberRequestMapper>());
        services.AddScoped<IOneWayMapper<GetMemberByIdRequest, GetMemberByIdQuery>>(sp => sp.GetRequiredService<MemberRequestMapper>());
        services.AddScoped<IOneWayMapper<SearchMembersRequest, SearchMembersQuery>>(sp => sp.GetRequiredService<MemberRequestMapper>());
    }

    private static void RegisterEquipmentRequestMappers(IServiceCollection services)
    {
        services.AddScoped<EquipmentRequestMapper>();
        services.AddScoped<IOneWayMapper<GetAllEquipmentRequest, GetAllEquipmentQuery>>(sp => sp.GetRequiredService<EquipmentRequestMapper>());
        services.AddScoped<IOneWayMapper<GetEquipmentByIdRequest, GetEquipmentByIdQuery>>(sp => sp.GetRequiredService<EquipmentRequestMapper>());
        services.AddScoped<IOneWayMapper<GetEquipmentByMemberRequest, GetEquipmentByMemberQuery>>(sp => sp.GetRequiredService<EquipmentRequestMapper>());
        services.AddScoped<IOneWayMapper<CreateEquipmentRequest, CreateEquipmentCommand>>(sp => sp.GetRequiredService<EquipmentRequestMapper>());
        services.AddScoped<IOneWayMapper<UpdateEquipmentRequest, UpdateEquipmentCommand>>(sp => sp.GetRequiredService<EquipmentRequestMapper>());
        services.AddScoped<IOneWayMapper<DeleteEquipmentRequest, DeleteEquipmentCommand>>(sp => sp.GetRequiredService<EquipmentRequestMapper>());
    }

    private static void RegisterMotorcycleRequestMappers(IServiceCollection services)
    {
        services.AddScoped<MotorcycleRequestMapper>();
        services.AddScoped<IOneWayMapper<GetAllMotorcyclesRequest, GetAllMotorcyclesQuery>>(sp => sp.GetRequiredService<MotorcycleRequestMapper>());
        services.AddScoped<IOneWayMapper<GetMotorcycleByIdRequest, GetMotorcycleByIdQuery>>(sp => sp.GetRequiredService<MotorcycleRequestMapper>());
        services.AddScoped<IOneWayMapper<GetMotorcyclesByMemberRequest, GetMotorcyclesByMemberQuery>>(sp => sp.GetRequiredService<MotorcycleRequestMapper>());
        services.AddScoped<IOneWayMapper<CreateMotorcycleRequest, CreateMotorcycleCommand>>(sp => sp.GetRequiredService<MotorcycleRequestMapper>());
        services.AddScoped<IOneWayMapper<UpdateMotorcycleRequest, UpdateMotorcycleCommand>>(sp => sp.GetRequiredService<MotorcycleRequestMapper>());
        services.AddScoped<IOneWayMapper<DeleteMotorcycleRequest, DeleteMotorcycleCommand>>(sp => sp.GetRequiredService<MotorcycleRequestMapper>());
    }

    private static void RegisterTrainingRequestMappers(IServiceCollection services)
    {
        services.AddScoped<TrainingRequestMapper>();
        services.AddScoped<IOneWayMapper<GetAllTrainingSessionsRequest, GetAllTrainingSessionsQuery>>(sp => sp.GetRequiredService<TrainingRequestMapper>());
        services.AddScoped<IOneWayMapper<GetTrainingSessionByIdRequest, GetTrainingSessionByIdQuery>>(sp => sp.GetRequiredService<TrainingRequestMapper>());
        services.AddScoped<IOneWayMapper<GetTrainingsByMemberRequest, GetTrainingsByMemberQuery>>(sp => sp.GetRequiredService<TrainingRequestMapper>());
        services.AddScoped<IOneWayMapper<GetTrainingByIdRequest, GetTrainingByIdQuery>>(sp => sp.GetRequiredService<TrainingRequestMapper>());
        services.AddScoped<IOneWayMapper<CreateTrainingSessionRequest, CreateTrainingSessionCommand>>(sp => sp.GetRequiredService<TrainingRequestMapper>());
        services.AddScoped<IOneWayMapper<UpdateTrainingSessionRequest, UpdateTrainingSessionCommand>>(sp => sp.GetRequiredService<TrainingRequestMapper>());
        services.AddScoped<IOneWayMapper<DeleteTrainingSessionRequest, DeleteTrainingSessionCommand>>(sp => sp.GetRequiredService<TrainingRequestMapper>());
        services.AddScoped<IOneWayMapper<CreateTrainingRequest, CreateTrainingCommand>>(sp => sp.GetRequiredService<TrainingRequestMapper>());
    }
}

