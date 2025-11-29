using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.API.Models;

public class Training
{
    public int Id { get; set; }

    [Required]
    public int MemberId { get; set; }

    [Required]
    public int MotorcycleId { get; set; }

    [Required]
    public int TrainingSessionId { get; set; }

    public bool IsCertificateIssued { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    [ForeignKey("MemberId")]
    public Member Member { get; set; } = null!;

    [ForeignKey("MotorcycleId")]
    public Motorcycle Motorcycle { get; set; } = null!;

    [ForeignKey("TrainingSessionId")]
    public TrainingSession TrainingSession { get; set; } = null!;
}

