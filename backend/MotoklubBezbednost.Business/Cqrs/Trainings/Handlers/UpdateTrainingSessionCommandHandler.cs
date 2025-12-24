using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class UpdateTrainingSessionCommandHandler : IRequestHandler<UpdateTrainingSessionCommand, TrainingSessionDto?>
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly ITwoWayDbMapper<TrainingSessionDb, TrainingSessionDto> _trainingSessionMapper;

    public UpdateTrainingSessionCommandHandler(ITrainingSessionRepository trainingSessionRepository, ITwoWayDbMapper<TrainingSessionDb, TrainingSessionDto> trainingSessionMapper)
    {
        _trainingSessionRepository = trainingSessionRepository;
        _trainingSessionMapper = trainingSessionMapper;
    }

    public async Task<TrainingSessionDto?> Handle(UpdateTrainingSessionCommand request, CancellationToken cancellationToken)
    {
        var existing = await _trainingSessionRepository.GetByIdWithDetailsAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        if (request.TheoryDate is not null) existing.TheoryDate = request.TheoryDate;
        if (request.PolygonDate is not null) existing.PolygonDate = request.PolygonDate;
        if (request.City is not null) existing.City = request.City;
        if (request.Price is not null) existing.Price = request.Price;
        if (request.Instructors is not null) existing.Instructors = request.Instructors;
        if (request.Note is not null) existing.Note = request.Note;
        if (request.LevelId is not null) existing.LevelId = request.LevelId.Value;

        existing.LastModificationTimestamp = request.LastModificationTimestamp;

        await _trainingSessionRepository.UpdateAsync(existing);
        return _trainingSessionMapper.ToDto(existing);
    }
}


