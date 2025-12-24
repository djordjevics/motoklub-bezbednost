using MediatR;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Commands;

public sealed class DeleteTrainingSessionCommand : IRequest<Unit>
{
    public int Id { get; init; }
}


