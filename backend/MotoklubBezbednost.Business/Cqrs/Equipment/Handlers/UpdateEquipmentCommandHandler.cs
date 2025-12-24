using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class UpdateEquipmentCommandHandler : IRequestHandler<UpdateEquipmentCommand, EquipmentDto?>
{
    private readonly IEquipmentRepository _equipmentRepository;

    private readonly ITwoWayDbMapper<EquipmentDb, EquipmentDto> _equipmentMapper;

    public UpdateEquipmentCommandHandler(IEquipmentRepository equipmentRepository, ITwoWayDbMapper<EquipmentDb, EquipmentDto> equipmentMapper)
    {
        _equipmentRepository = equipmentRepository;
        _equipmentMapper = equipmentMapper;
    }

    public async Task<EquipmentDto?> Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var existing = await _equipmentRepository.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        var dto = new EquipmentDto
        {
            Id = request.Id,
            Pants = request.Pants,
            Jacket = request.Jacket,
            Vest = request.Vest,
            WorkShirt = request.WorkShirt,
            FormalShirt = request.FormalShirt,
            Note = request.Note
        };

        _equipmentMapper.ToEntity(dto, existing);
        existing.LastModificationTimestamp = request.LastModificationTimestamp;

        await _equipmentRepository.UpdateAsync(existing);
        return _equipmentMapper.ToDto(existing);
    }
}


