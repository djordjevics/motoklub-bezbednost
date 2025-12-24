using MediatR;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Commands;

public sealed class DeleteEquipmentCommand : IRequest<Unit>
{
    public int Id { get; init; }
}


