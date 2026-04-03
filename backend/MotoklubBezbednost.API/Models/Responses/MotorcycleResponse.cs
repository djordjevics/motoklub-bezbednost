namespace MotoklubBezbednost.API.Models.Responses;

public class MotorcycleResponse
{
    public int Id { get; set; }
    public string? BrandName { get; set; }
    public string? CommercialName { get; set; }
    public string? ModelName { get; set; }
    public int? EngineDisplacment { get; set; }
    public int? EnginePower { get; set; }
    public string? Color { get; set; }
    public string? RegisterPlate { get; set; }
    public DateTime CreationTimestamp { get; set; }
    public DateTime? LastModificationTimestamp { get; set; }
}
