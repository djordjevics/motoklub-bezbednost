using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class GetTrainingByIdQueryHandler : IRequestHandler<GetTrainingByIdQuery, TrainingDto?>
{
    private readonly ITrainingRepository _trainingRepository;

    private readonly ITwoWayDbMapper<TrainingDb, TrainingDto> _trainingMapper;

    public GetTrainingByIdQueryHandler(ITrainingRepository trainingRepository, ITwoWayDbMapper<TrainingDb, TrainingDto> trainingMapper)
    {
        _trainingRepository = trainingRepository;
        _trainingMapper = trainingMapper;
    }

    public async Task<TrainingDto?> Handle(GetTrainingByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _trainingRepository.GetByIdWithDetailsAsync(request.Id);
        return entity is null ? null : _trainingMapper.ToDto(entity);
    }
}


