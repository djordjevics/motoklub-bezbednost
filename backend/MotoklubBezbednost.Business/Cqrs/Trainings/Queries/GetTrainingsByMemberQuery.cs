using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Queries;

public sealed class GetTrainingsByMemberQuery : IRequest<IEnumerable<TrainingDto>>
{
    public int MemberId { get; init; }
}


