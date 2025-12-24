using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;

public sealed class UpdateMotorcycleCommand : IRequest<MotorcycleDto?>
{
    public int Id { get; init; }
    public string? BrandName { get; init; }
    public string? CommercialName { get; init; }
    public string? ModelName { get; init; }
    public int? EngineDisplacment { get; init; }
    public int? EnginePower { get; init; }
    public string? Color { get; init; }
    public string? RegisterPlate { get; init; }
    public int? MemberId { get; init; }
    public DateTime LastModificationTimestamp { get; init; }
}


