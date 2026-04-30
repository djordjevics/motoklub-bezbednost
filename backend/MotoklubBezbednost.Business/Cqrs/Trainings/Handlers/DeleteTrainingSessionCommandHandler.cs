using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class DeleteTrainingSessionCommandHandler : IRequestHandler<DeleteTrainingSessionCommand, Unit>
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTrainingSessionCommandHandler(ITrainingSessionRepository trainingSessionRepository, IUnitOfWork unitOfWork)
    {
        _trainingSessionRepository = trainingSessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteTrainingSessionCommand request, CancellationToken cancellationToken)
    {
        await _trainingSessionRepository.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}


