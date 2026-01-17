using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Commands;

public sealed class UpdateTrainingSessionCommand : IRequest<TrainingSessionDto?>
{
    public int Id { get; init; }
    public DateTime? TheoryDate { get; init; }
    public DateTime? PolygonDate { get; init; }
    public string? City { get; init; }
    public int? Price { get; init; }
    public string? Instructors { get; init; }
    public string? Note { get; init; }
    public int? LevelId { get; init; }
}


