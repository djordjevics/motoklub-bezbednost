using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;
using MotoklubBezbednost.Business.Mappings;

namespace MotoklubBezbednost.API.Mappers;

/// <summary>
/// Injectable mapper for Equipment API requests to CQRS commands/queries.
/// </summary>
public sealed class EquipmentRequestMapper :
    IOneWayMapper<GetAllEquipmentRequest, GetAllEquipmentQuery>,
    IOneWayMapper<GetEquipmentByIdRequest, GetEquipmentByIdQuery>,
    IOneWayMapper<GetEquipmentByMemberRequest, GetEquipmentByMemberQuery>,
    IOneWayMapper<CreateEquipmentRequest, CreateEquipmentCommand>,
    IOneWayMapper<UpdateEquipmentRequest, UpdateEquipmentCommand>,
    IOneWayMapper<DeleteEquipmentRequest, DeleteEquipmentCommand>
{
    // IOneWayMapper<GetAllEquipmentRequest, GetAllEquipmentQuery>
    GetAllEquipmentQuery IOneWayMapper<GetAllEquipmentRequest, GetAllEquipmentQuery>.Map(GetAllEquipmentRequest source)
        => new GetAllEquipmentQuery();

    // IOneWayMapper<GetEquipmentByIdRequest, GetEquipmentByIdQuery>
    GetEquipmentByIdQuery IOneWayMapper<GetEquipmentByIdRequest, GetEquipmentByIdQuery>.Map(GetEquipmentByIdRequest source)
        => new GetEquipmentByIdQuery { Id = source.Id };

    // IOneWayMapper<GetEquipmentByMemberRequest, GetEquipmentByMemberQuery>
    GetEquipmentByMemberQuery IOneWayMapper<GetEquipmentByMemberRequest, GetEquipmentByMemberQuery>.Map(GetEquipmentByMemberRequest source)
        => new GetEquipmentByMemberQuery { MemberId = source.MemberId };

    // IOneWayMapper<CreateEquipmentRequest, CreateEquipmentCommand>
    CreateEquipmentCommand IOneWayMapper<CreateEquipmentRequest, CreateEquipmentCommand>.Map(CreateEquipmentRequest source)
        => new CreateEquipmentCommand
        {
            Pants = source.Pants,
            Jacket = source.Jacket,
            Vest = source.Vest,
            WorkShirt = source.WorkShirt,
            FormalShirt = source.FormalShirt,
            Note = source.Note,
            CreationTimestamp = DateTime.UtcNow
        };

    // IOneWayMapper<UpdateEquipmentRequest, UpdateEquipmentCommand>
    UpdateEquipmentCommand IOneWayMapper<UpdateEquipmentRequest, UpdateEquipmentCommand>.Map(UpdateEquipmentRequest source)
        => new UpdateEquipmentCommand
        {
            Id = source.Id,
            Pants = source.Pants,
            Jacket = source.Jacket,
            Vest = source.Vest,
            WorkShirt = source.WorkShirt,
            FormalShirt = source.FormalShirt,
            Note = source.Note,
            LastModificationTimestamp = DateTime.UtcNow
        };

    // IOneWayMapper<DeleteEquipmentRequest, DeleteEquipmentCommand>
    DeleteEquipmentCommand IOneWayMapper<DeleteEquipmentRequest, DeleteEquipmentCommand>.Map(DeleteEquipmentRequest source)
        => new DeleteEquipmentCommand { Id = source.Id };
}
