using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Business.Dtos;

public class MotorcycleDto
{
    public int Id { get; set; }

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

    public DateTime CreationTimestamp { get; set; }

    public DateTime? LastModificationTimestamp { get; set; }
}


