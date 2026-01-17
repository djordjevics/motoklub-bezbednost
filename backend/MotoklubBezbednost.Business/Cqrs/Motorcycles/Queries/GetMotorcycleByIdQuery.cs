using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;

public sealed class GetMotorcycleByIdQuery : IRequest<MotorcycleDto?>
{
    public int Id { get; init; }
}


