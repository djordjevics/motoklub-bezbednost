using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class CreateEquipmentCommandHandler : IRequestHandler<CreateEquipmentCommand, EquipmentDto>
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly ITwoWayDbMapper<EquipmentDb, EquipmentDto> _equipmentMapper;

    public CreateEquipmentCommandHandler(IEquipmentRepository equipmentRepository, ITwoWayDbMapper<EquipmentDb, EquipmentDto> equipmentMapper)
    {
        _equipmentRepository = equipmentRepository;
        _equipmentMapper = equipmentMapper;
    }

    public async Task<EquipmentDto> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var dto = new EquipmentDto
        {
            Pants = request.Pants,
            Jacket = request.Jacket,
            Vest = request.Vest,
            WorkShirt = request.WorkShirt,
            FormalShirt = request.FormalShirt,
            Note = request.Note
        };

        var entity = _equipmentMapper.ToEntity(dto);
        entity.CreationTimestamp = request.CreationTimestamp;

        var created = await _equipmentRepository.AddAsync(entity);
        return _equipmentMapper.ToDto(created);
    }
}


