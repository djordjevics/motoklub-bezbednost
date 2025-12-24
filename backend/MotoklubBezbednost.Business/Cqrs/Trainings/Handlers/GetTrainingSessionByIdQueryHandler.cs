using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class GetTrainingSessionByIdQueryHandler : IRequestHandler<GetTrainingSessionByIdQuery, TrainingSessionDto?>
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;

    private readonly ITwoWayDbMapper<TrainingSessionDb, TrainingSessionDto> _trainingSessionMapper;

    public GetTrainingSessionByIdQueryHandler(ITrainingSessionRepository trainingSessionRepository, ITwoWayDbMapper<TrainingSessionDb, TrainingSessionDto> trainingSessionMapper)
    {
        _trainingSessionRepository = trainingSessionRepository;
        _trainingSessionMapper = trainingSessionMapper;
    }

    public async Task<TrainingSessionDto?> Handle(GetTrainingSessionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _trainingSessionRepository.GetByIdWithDetailsAsync(request.Id);
        return entity is null ? null : _trainingSessionMapper.ToDto(entity);
    }
}


