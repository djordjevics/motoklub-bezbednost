namespace MotoklubBezbednost.API.Models.Requests;

public class UpdateTrainingRequest
{
    public int Id { get; set; }
    public bool? RepeatingAttendance { get; set; }
    public bool? IsCertificateIssued { get; set; }
    public string? Note { get; set; }
    public int? MotorcycleId { get; set; }
}
