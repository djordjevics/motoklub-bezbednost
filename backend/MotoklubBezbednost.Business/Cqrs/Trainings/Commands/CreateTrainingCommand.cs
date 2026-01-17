using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Commands;

public sealed class CreateTrainingCommand : IRequest<TrainingDto>
{
    public bool IsCertificateIssued { get; init; }
    public string? Note { get; init; }
    public int MemberId { get; init; }
    public int MotorcycleId { get; init; }
    public int TrainingSessionId { get; init; }
}


