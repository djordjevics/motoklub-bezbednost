using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Queries;

public sealed class GetTrainingByIdQuery : IRequest<TrainingDto?>
{
    public int Id { get; init; }
}


