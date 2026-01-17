using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Commands;

public sealed class CreateTrainingSessionCommand : IRequest<TrainingSessionDto>
{
    public DateTime TheoryDate { get; init; }
    public DateTime PolygonDate { get; init; }
    public string City { get; init; } = null!;
    public int? Price { get; init; }
    public string? Instructors { get; init; }
    public string? Note { get; init; }
    public int LevelId { get; init; }
}


