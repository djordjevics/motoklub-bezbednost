using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;

public sealed class GetAllMotorcyclesQuery : IRequest<IEnumerable<MotorcycleDto>>
{
}


