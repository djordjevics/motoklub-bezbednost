using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.API.Requests;

public class CreateMotorcycleRequest
{
    [Required]
    public string BrandName { get; set; }
    public string? CommercialName { get; set; }
    public string? ModelName { get; set; }
    public int? EngineDisplacment { get; set; }
    public int? EnginePower { get; set; }
    public string? Color { get; set; }
    public string? RegisterPlate { get; set; }
    [Required]
    public int MemberId { get; set; }
}


