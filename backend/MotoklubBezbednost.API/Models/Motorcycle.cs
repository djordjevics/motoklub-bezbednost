using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.API.Models;

public class Motorcycle
{
    public int Id { get; set; }

    [Required]
    public int MemberId { get; set; }

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

    // Navigation properties
    [ForeignKey("MemberId")]
    public Member Member { get; set; } = null!;

    public ICollection<Training> Trainings { get; set; } = new List<Training>();
}

