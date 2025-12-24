using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class GetTrainingsByMemberQueryHandler : IRequestHandler<GetTrainingsByMemberQuery, IEnumerable<TrainingDto>>
{
    private readonly ITrainingRepository _trainingRepository;

    private readonly ITwoWayDbMapper<TrainingDb, TrainingDto> _trainingMapper;

    public GetTrainingsByMemberQueryHandler(ITrainingRepository trainingRepository, ITwoWayDbMapper<TrainingDb, TrainingDto> trainingMapper)
    {
        _trainingRepository = trainingRepository;
        _trainingMapper = trainingMapper;
    }

    public async Task<IEnumerable<TrainingDto>> Handle(GetTrainingsByMemberQuery request, CancellationToken cancellationToken)
    {
        var entities = await _trainingRepository.GetByMemberIdAsync(request.MemberId);
        return _trainingMapper.ToDto(entities);
    }
}


