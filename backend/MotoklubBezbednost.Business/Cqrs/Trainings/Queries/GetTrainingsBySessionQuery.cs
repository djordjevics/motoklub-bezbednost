using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Queries;

public sealed class GetTrainingsBySessionQuery : IRequest<IEnumerable<TrainingDto>>
{
    public int TrainingSessionId { get; init; }
}
