using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class GetAllTrainingSessionsQueryHandler : IRequestHandler<GetAllTrainingSessionsQuery, IEnumerable<TrainingSessionDto>>
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;

    private readonly ITwoWayDbMapper<TrainingSessionDb, TrainingSessionDto> _trainingSessionMapper;

    public GetAllTrainingSessionsQueryHandler(ITrainingSessionRepository trainingSessionRepository, ITwoWayDbMapper<TrainingSessionDb, TrainingSessionDto> trainingSessionMapper)
    {
        _trainingSessionRepository = trainingSessionRepository;
        _trainingSessionMapper = trainingSessionMapper;
    }

    public async Task<IEnumerable<TrainingSessionDto>> Handle(GetAllTrainingSessionsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _trainingSessionRepository.GetAllWithDetailsAsync();
        return _trainingSessionMapper.ToDto(entities);
    }
}


