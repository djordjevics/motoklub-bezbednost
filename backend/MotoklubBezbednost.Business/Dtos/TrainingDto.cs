using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Business.Dtos;

public class TrainingDto
{
    public int Id { get; set; }

    public bool IsCertificateIssued { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    public MemberDto? Member { get; set; }

    public MotorcycleDto? Motorcycle { get; set; }

    public TrainingSessionDto? TrainingSession { get; set; }
}


