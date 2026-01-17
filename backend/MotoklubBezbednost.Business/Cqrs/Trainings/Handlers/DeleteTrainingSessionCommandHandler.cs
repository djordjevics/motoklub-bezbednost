using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class DeleteTrainingSessionCommandHandler : IRequestHandler<DeleteTrainingSessionCommand, Unit>
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;

    public DeleteTrainingSessionCommandHandler(ITrainingSessionRepository trainingSessionRepository)
    {
        _trainingSessionRepository = trainingSessionRepository;
    }

    public async Task<Unit> Handle(DeleteTrainingSessionCommand request, CancellationToken cancellationToken)
    {
        await _trainingSessionRepository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}


