using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Queries;

public sealed class GetTrainingSessionByIdQuery : IRequest<TrainingSessionDto?>
{
    public int Id { get; init; }
}


