using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;

public sealed class CreateMotorcycleCommand : IRequest<MotorcycleDto>
{
    public string BrandName { get; init; } = null!;
    public string? CommercialName { get; init; }
    public string? ModelName { get; init; }
    public int? EngineDisplacment { get; init; }
    public int? EnginePower { get; init; }
    public string? Color { get; init; }
    public string? RegisterPlate { get; init; }
    public int MemberId { get; init; }
}


