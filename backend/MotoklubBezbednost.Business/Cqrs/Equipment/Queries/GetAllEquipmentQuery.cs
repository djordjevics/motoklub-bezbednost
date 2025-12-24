using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Queries;

public sealed class GetAllEquipmentQuery : IRequest<IEnumerable<EquipmentDto>>
{
}


