using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class CreateTrainingSessionCommandHandler : IRequestHandler<CreateTrainingSessionCommand, TrainingSessionDto>
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly IMapper _mapper;

    public CreateTrainingSessionCommandHandler(ITrainingSessionRepository trainingSessionRepository, IMapper mapper)
    {
        _trainingSessionRepository = trainingSessionRepository;
        _mapper = mapper;
    }

    public async Task<TrainingSessionDto> Handle(CreateTrainingSessionCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToDbModel();
        var created = await _trainingSessionRepository.AddAsync(entity);
        return _mapper.Map<Data.Models.TrainingSessionDb, TrainingSessionDto>(created);
    }
}


