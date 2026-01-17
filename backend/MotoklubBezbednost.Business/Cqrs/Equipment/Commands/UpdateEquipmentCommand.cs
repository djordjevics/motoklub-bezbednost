using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Commands;

public sealed class UpdateEquipmentCommand : IRequest<EquipmentDto?>
{
    public int Id { get; init; }
    public bool Pants { get; init; }
    public bool Jacket { get; init; }
    public bool Vest { get; init; }
    public bool WorkShirt { get; init; }
    public bool FormalShirt { get; init; }
    public string? Note { get; init; }
}


