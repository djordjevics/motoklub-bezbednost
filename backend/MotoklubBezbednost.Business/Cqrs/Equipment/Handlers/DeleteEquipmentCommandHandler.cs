using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class DeleteEquipmentCommandHandler : IRequestHandler<DeleteEquipmentCommand, Unit>
{
    private readonly IEquipmentRepository _equipmentRepository;

    public DeleteEquipmentCommandHandler(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<Unit> Handle(DeleteEquipmentCommand request, CancellationToken cancellationToken)
    {
        await _equipmentRepository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}


