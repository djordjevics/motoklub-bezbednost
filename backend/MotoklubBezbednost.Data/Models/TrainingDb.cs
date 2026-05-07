using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.Data.Models;

public class TrainingDb : DbModel
{
    /// <summary>True when this attendee participates again (renewal / repeating presence).</summary>
    public bool RepeatingAttendance { get; set; }

    public bool IsCertificateIssued { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Foreign keys
    [Required]
    public int MemberId { get; set; }

    [Required]
    public int MotorcycleId { get; set; }

    [Required]
    public int TrainingSessionId { get; set; }

    // Navigation properties
    public virtual MotorcycleDb Motorcycle { get; set; } = null!;

    public virtual TrainingSessionDb TrainingSession { get; set; } = null!;
}


