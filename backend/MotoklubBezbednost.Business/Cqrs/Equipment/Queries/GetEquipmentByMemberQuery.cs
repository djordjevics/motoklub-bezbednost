using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Queries;

public sealed class GetEquipmentByMemberQuery : IRequest<IEnumerable<EquipmentDto>>
{
    public int MemberId { get; init; }
}


