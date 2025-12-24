using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Queries;

public sealed class GetAllTrainingSessionsQuery : IRequest<IEnumerable<TrainingSessionDto>>
{
}


