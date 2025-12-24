using MediatR;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Handlers;

public sealed class DeleteMotorcycleCommandHandler : IRequestHandler<DeleteMotorcycleCommand, Unit>
{
    private readonly IMotorcycleRepository _motorcycleRepository;

    public DeleteMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository)
    {
        _motorcycleRepository = motorcycleRepository;
    }

    public async Task<Unit> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
    {
        await _motorcycleRepository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}


