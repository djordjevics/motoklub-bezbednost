using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;
using MotoklubBezbednost.Business.Dtos;
using AutoMapper;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class GetTrainingsByMemberQueryHandler : IRequestHandler<GetTrainingsByMemberQuery, IEnumerable<TrainingDto>>
{
    private readonly ITrainingRepository _trainingRepository;
    private readonly IMapper _mapper;

    public GetTrainingsByMemberQueryHandler(ITrainingRepository trainingRepository, IMapper mapper)
    {
        _trainingRepository = trainingRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TrainingDto>> Handle(GetTrainingsByMemberQuery request, CancellationToken cancellationToken)
    {
        var entities = await _trainingRepository.GetByMemberIdAsync(request.MemberId);
        return entities.Select(e => _mapper.Map<TrainingDto>(e));
    }
}


