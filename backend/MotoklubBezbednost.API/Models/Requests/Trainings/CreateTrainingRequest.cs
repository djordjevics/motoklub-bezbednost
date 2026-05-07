namespace MotoklubBezbednost.API.Models.Requests;

public class CreateTrainingRequest
{
    public bool RepeatingAttendance { get; set; }
    public bool IsCertificateIssued { get; set; }
    public string? Note { get; set; }
    public int MemberId { get; set; }
    public int MotorcycleId { get; set; }
    public int TrainingSessionId { get; set; }
}


