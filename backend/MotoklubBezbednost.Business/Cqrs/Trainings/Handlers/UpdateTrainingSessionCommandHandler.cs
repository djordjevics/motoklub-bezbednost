using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class UpdateTrainingSessionCommandHandler : IRequestHandler<UpdateTrainingSessionCommand, TrainingSessionDto?>
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTrainingSessionCommandHandler(ITrainingSessionRepository trainingSessionRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _trainingSessionRepository = trainingSessionRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TrainingSessionDto?> Handle(UpdateTrainingSessionCommand request, CancellationToken cancellationToken)
    {
        var existing = await _trainingSessionRepository.GetByIdWithDetailsAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(request, existing);
        await _trainingSessionRepository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TrainingSessionDto>(existing);
    }
}


