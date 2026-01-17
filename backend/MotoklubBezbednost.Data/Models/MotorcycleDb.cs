using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.Data.Models;

public class MotorcycleDb : DbModel
{
    [StringLength(100)]
    public string? BrandName { get; set; }

    [StringLength(100)]
    public string? CommercialName { get; set; }

    [StringLength(100)]
    public string? ModelName { get; set; }

    public int? EngineDisplacment { get; set; }

    public int? EnginePower { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    [StringLength(20)]
    public string? RegisterPlate { get; set; }

    // Foreign keys
    [Required]
    public int MemberId { get; set; }

    // Navigation properties
    public virtual MemberDb Member { get; set; } = null!;

    public virtual ICollection<TrainingDb> Trainings { get; set; } = new List<TrainingDb>();
}


