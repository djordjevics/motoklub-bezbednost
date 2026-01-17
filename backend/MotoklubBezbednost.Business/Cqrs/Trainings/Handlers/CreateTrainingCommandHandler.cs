using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class CreateTrainingCommandHandler : IRequestHandler<CreateTrainingCommand, TrainingDto>
{
    private readonly ITrainingRepository _trainingRepository;
    private readonly IMapper _mapper;

    public CreateTrainingCommandHandler(ITrainingRepository trainingRepository, IMapper mapper)
    {
        _trainingRepository = trainingRepository;
        _mapper = mapper;
    }

    public async Task<TrainingDto> Handle(CreateTrainingCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToDbModel();
        var created = await _trainingRepository.AddAsync(entity);
        return _mapper.Map<Data.Models.TrainingDb, TrainingDto>(created);
    }
}


