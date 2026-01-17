using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class UpdateTrainingSessionCommandHandler : IRequestHandler<UpdateTrainingSessionCommand, TrainingSessionDto?>
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly IMapper _mapper;

    public UpdateTrainingSessionCommandHandler(ITrainingSessionRepository trainingSessionRepository, IMapper mapper)
    {
        _trainingSessionRepository = trainingSessionRepository;
        _mapper = mapper;
    }

    public async Task<TrainingSessionDto?> Handle(UpdateTrainingSessionCommand request, CancellationToken cancellationToken)
    {
        var existing = await _trainingSessionRepository.GetByIdWithDetailsAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        request.ApplyTo(existing);
        await _trainingSessionRepository.UpdateAsync(existing);
        return _mapper.Map<Data.Models.TrainingSessionDb, TrainingSessionDto>(existing);
    }
}


