using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;

public sealed class GetMotorcyclesByMemberQuery : IRequest<IEnumerable<MotorcycleDto>>
{
    public int MemberId { get; init; }
}


