using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Queries;

public sealed class GetEquipmentByIdQuery : IRequest<EquipmentDto?>
{
    public int Id { get; init; }
}


