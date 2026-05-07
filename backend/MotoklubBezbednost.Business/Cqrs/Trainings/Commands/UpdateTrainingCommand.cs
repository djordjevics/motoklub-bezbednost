using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Commands;

public sealed class UpdateTrainingCommand : IRequest<TrainingDto?>
{
    public int Id { get; init; }
    public bool? RepeatingAttendance { get; init; }
    public bool? IsCertificateIssued { get; init; }
    public string? Note { get; init; }
    public int? MotorcycleId { get; init; }
}
