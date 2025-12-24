using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class CreateTrainingSessionCommandHandler : IRequestHandler<CreateTrainingSessionCommand, TrainingSessionDto>
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;

    private readonly ITwoWayDbMapper<TrainingSessionDb, TrainingSessionDto> _trainingSessionMapper;

    public CreateTrainingSessionCommandHandler(ITrainingSessionRepository trainingSessionRepository, ITwoWayDbMapper<TrainingSessionDb, TrainingSessionDto> trainingSessionMapper)
    {
        _trainingSessionRepository = trainingSessionRepository;
        _trainingSessionMapper = trainingSessionMapper;
    }

    public async Task<TrainingSessionDto> Handle(CreateTrainingSessionCommand request, CancellationToken cancellationToken)
    {
        var dto = new TrainingSessionDto
        {
            TheoryDate = request.TheoryDate,
            PolygonDate = request.PolygonDate,
            City = request.City,
            Price = request.Price,
            Instructors = request.Instructors,
            Note = request.Note
        };

        var entity = _trainingSessionMapper.ToEntity(dto);
        entity.CreationTimestamp = request.CreationTimestamp;
        entity.LevelId = request.LevelId;

        var created = await _trainingSessionRepository.AddAsync(entity);
        return _trainingSessionMapper.ToDto(created);
    }
}


