using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.API.Models;

public class Training
{
    public int Id { get; set; }

    public bool IsCertificateIssued { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    [Required]
    public Member Member { get; set; } = null!;

    [Required]
    public Motorcycle Motorcycle { get; set; } = null!;

    [Required]
    public TrainingSession TrainingSession { get; set; } = null!;
}

