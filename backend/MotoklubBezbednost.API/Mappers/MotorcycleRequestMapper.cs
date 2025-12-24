using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;
using MotoklubBezbednost.Business.Mappings;

namespace MotoklubBezbednost.API.Mappers;

/// <summary>
/// Injectable mapper for Motorcycle API requests to CQRS commands/queries.
/// </summary>
public sealed class MotorcycleRequestMapper :
    IOneWayMapper<GetAllMotorcyclesRequest, GetAllMotorcyclesQuery>,
    IOneWayMapper<GetMotorcycleByIdRequest, GetMotorcycleByIdQuery>,
    IOneWayMapper<GetMotorcyclesByMemberRequest, GetMotorcyclesByMemberQuery>,
    IOneWayMapper<CreateMotorcycleRequest, CreateMotorcycleCommand>,
    IOneWayMapper<UpdateMotorcycleRequest, UpdateMotorcycleCommand>,
    IOneWayMapper<DeleteMotorcycleRequest, DeleteMotorcycleCommand>
{
    // IOneWayMapper<GetAllMotorcyclesRequest, GetAllMotorcyclesQuery>
    GetAllMotorcyclesQuery IOneWayMapper<GetAllMotorcyclesRequest, GetAllMotorcyclesQuery>.Map(GetAllMotorcyclesRequest source)
        => new GetAllMotorcyclesQuery();

    // IOneWayMapper<GetMotorcycleByIdRequest, GetMotorcycleByIdQuery>
    GetMotorcycleByIdQuery IOneWayMapper<GetMotorcycleByIdRequest, GetMotorcycleByIdQuery>.Map(GetMotorcycleByIdRequest source)
        => new GetMotorcycleByIdQuery { Id = source.Id };

    // IOneWayMapper<GetMotorcyclesByMemberRequest, GetMotorcyclesByMemberQuery>
    GetMotorcyclesByMemberQuery IOneWayMapper<GetMotorcyclesByMemberRequest, GetMotorcyclesByMemberQuery>.Map(GetMotorcyclesByMemberRequest source)
        => new GetMotorcyclesByMemberQuery { MemberId = source.MemberId };

    // IOneWayMapper<CreateMotorcycleRequest, CreateMotorcycleCommand>
    CreateMotorcycleCommand IOneWayMapper<CreateMotorcycleRequest, CreateMotorcycleCommand>.Map(CreateMotorcycleRequest source)
        => new CreateMotorcycleCommand
        {
            BrandName = source.BrandName,
            CommercialName = source.CommercialName,
            ModelName = source.ModelName,
            EngineDisplacment = source.EngineDisplacment,
            EnginePower = source.EnginePower,
            Color = source.Color,
            RegisterPlate = source.RegisterPlate,
            MemberId = source.MemberId,
            CreationTimestamp = DateTime.UtcNow
        };

    // IOneWayMapper<UpdateMotorcycleRequest, UpdateMotorcycleCommand>
    UpdateMotorcycleCommand IOneWayMapper<UpdateMotorcycleRequest, UpdateMotorcycleCommand>.Map(UpdateMotorcycleRequest source)
        => new UpdateMotorcycleCommand
        {
            Id = source.Id,
            BrandName = source.BrandName,
            CommercialName = source.CommercialName,
            ModelName = source.ModelName,
            EngineDisplacment = source.EngineDisplacment,
            EnginePower = source.EnginePower,
            Color = source.Color,
            RegisterPlate = source.RegisterPlate,
            MemberId = source.MemberId,
            LastModificationTimestamp = DateTime.UtcNow
        };

    // IOneWayMapper<DeleteMotorcycleRequest, DeleteMotorcycleCommand>
    DeleteMotorcycleCommand IOneWayMapper<DeleteMotorcycleRequest, DeleteMotorcycleCommand>.Map(DeleteMotorcycleRequest source)
        => new DeleteMotorcycleCommand { Id = source.Id };
}
