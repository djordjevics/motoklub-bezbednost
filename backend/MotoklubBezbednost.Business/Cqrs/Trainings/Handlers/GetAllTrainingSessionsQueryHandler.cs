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
    private readonly IMapper _mapper;

    public GetAllTrainingSessionsQueryHandler(ITrainingSessionRepository trainingSessionRepository, IMapper mapper)
    {
        _trainingSessionRepository = trainingSessionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TrainingSessionDto>> Handle(GetAllTrainingSessionsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _trainingSessionRepository.GetAllWithDetailsAsync();
        return entities.Select(e => _mapper.Map<TrainingSessionDb, TrainingSessionDto>(e));
    }
}


