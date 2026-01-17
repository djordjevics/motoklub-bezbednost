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
    private readonly IMapper _mapper;

    public GetTrainingByIdQueryHandler(ITrainingRepository trainingRepository, IMapper mapper)
    {
        _trainingRepository = trainingRepository;
        _mapper = mapper;
    }

    public async Task<TrainingDto?> Handle(GetTrainingByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _trainingRepository.GetByIdWithDetailsAsync(request.Id);
        return entity is null ? null : _mapper.Map<TrainingDb, TrainingDto>(entity);
    }
}


