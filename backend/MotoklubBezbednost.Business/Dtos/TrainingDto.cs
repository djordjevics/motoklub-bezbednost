using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Business.Dtos;

public class TrainingDto
{
    public int Id { get; set; }

    public bool RepeatingAttendance { get; set; }

    public bool IsCertificateIssued { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    public DateTime CreationTimestamp { get; set; }

    public DateTime? LastModificationTimestamp { get; set; }

    public int MemberId { get; set; }

    public int MotorcycleId { get; set; }

    public int TrainingSessionId { get; set; }

    public MemberDto? Member { get; set; }

    public MotorcycleDto? Motorcycle { get; set; }

    public TrainingSessionDto? TrainingSession { get; set; }
}


