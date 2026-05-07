using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class GetTrainingsBySessionQueryHandler : IRequestHandler<GetTrainingsBySessionQuery, IEnumerable<TrainingDto>>
{
    private readonly ITrainingRepository _trainingRepository;
    private readonly IMapper _mapper;

    public GetTrainingsBySessionQueryHandler(ITrainingRepository trainingRepository, IMapper mapper)
    {
        _trainingRepository = trainingRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TrainingDto>> Handle(GetTrainingsBySessionQuery request, CancellationToken cancellationToken)
    {
        var entities = await _trainingRepository.GetBySessionIdAsync(request.TrainingSessionId);
        return entities.Select(e => _mapper.Map<TrainingDto>(e));
    }
}
