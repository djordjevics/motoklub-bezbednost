using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.Data.Models;

public class TrainingDb
{
    public int Id { get; set; }

    public bool IsCertificateIssued { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    [Required]
    public MemberDb Member { get; set; } = null!;

    [Required]
    public MotorcycleDb Motorcycle { get; set; } = null!;

    [Required]
    public TrainingSessionDb TrainingSession { get; set; } = null!;
}


