namespace MotoklubBezbednost.API.Models.Responses;

public class TrainingResponse
{
    public int Id { get; set; }
    public bool RepeatingAttendance { get; set; }
    public bool IsCertificateIssued { get; set; }
    public string? Note { get; set; }
    public DateTime CreationTimestamp { get; set; }
    public DateTime? LastModificationTimestamp { get; set; }
    public int MemberId { get; set; }
    public int MotorcycleId { get; set; }
    public int TrainingSessionId { get; set; }
    public MemberResponse? Member { get; set; }
    public MotorcycleResponse? Motorcycle { get; set; }
    public TrainingSessionResponse? TrainingSession { get; set; }
}
