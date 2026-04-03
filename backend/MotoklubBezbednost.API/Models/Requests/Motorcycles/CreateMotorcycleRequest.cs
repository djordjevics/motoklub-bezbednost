namespace MotoklubBezbednost.API.Models.Requests;

public class CreateMotorcycleRequest
{
    public string? BrandName { get; set; }
    public string? CommercialName { get; set; }
    public string? ModelName { get; set; }
    public int? EngineDisplacment { get; set; }
    public int? EnginePower { get; set; }
    public string? Color { get; set; }
    public string? RegisterPlate { get; set; }
    public int MemberId { get; set; }
}
