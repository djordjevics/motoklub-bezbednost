using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class GetEquipmentByIdQueryHandler : IRequestHandler<GetEquipmentByIdQuery, EquipmentDto?>
{
    private readonly IEquipmentRepository _equipmentRepository;

    private readonly ITwoWayDbMapper<EquipmentDb, EquipmentDto> _equipmentMapper;

    public GetEquipmentByIdQueryHandler(IEquipmentRepository equipmentRepository, ITwoWayDbMapper<EquipmentDb, EquipmentDto> equipmentMapper)
    {
        _equipmentRepository = equipmentRepository;
        _equipmentMapper = equipmentMapper;
    }

    public async Task<EquipmentDto?> Handle(GetEquipmentByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _equipmentRepository.GetByIdAsync(request.Id);
        return entity is null ? null : _equipmentMapper.ToDto(entity);
    }
}


