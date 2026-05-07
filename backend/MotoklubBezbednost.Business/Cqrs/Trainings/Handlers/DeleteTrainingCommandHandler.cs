using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class DeleteTrainingCommandHandler : IRequestHandler<DeleteTrainingCommand, Unit>
{
    private readonly ITrainingRepository _trainingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTrainingCommandHandler(ITrainingRepository trainingRepository, IUnitOfWork unitOfWork)
    {
        _trainingRepository = trainingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteTrainingCommand request, CancellationToken cancellationToken)
    {
        await _trainingRepository.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
