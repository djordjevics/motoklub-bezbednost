using MediatR;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;

public sealed class DeleteMotorcycleCommand : IRequest<Unit>
{
    public int Id { get; init; }
}


