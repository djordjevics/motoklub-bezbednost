using MediatR;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Commands;

public sealed class DeleteTrainingCommand : IRequest<Unit>
{
    public int Id { get; init; }
}
