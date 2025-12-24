using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class GetAllEquipmentQueryHandler : IRequestHandler<GetAllEquipmentQuery, IEnumerable<EquipmentDto>>
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly ITwoWayDbMapper<EquipmentDb, EquipmentDto> _equipmentMapper;

    public GetAllEquipmentQueryHandler(IEquipmentRepository equipmentRepository, ITwoWayDbMapper<EquipmentDb, EquipmentDto> equipmentMapper)
    {
        _equipmentRepository = equipmentRepository;
        _equipmentMapper = equipmentMapper;
    }

    public async Task<IEnumerable<EquipmentDto>> Handle(GetAllEquipmentQuery request, CancellationToken cancellationToken)
    {
        var entities = await _equipmentRepository.GetAllWithMemberAsync();
        return _equipmentMapper.ToDto(entities);
    }
}


