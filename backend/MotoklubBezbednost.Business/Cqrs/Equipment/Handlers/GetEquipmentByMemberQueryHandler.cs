using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class GetEquipmentByMemberQueryHandler : IRequestHandler<GetEquipmentByMemberQuery, IEnumerable<EquipmentDto>>
{
    private readonly IEquipmentRepository _equipmentRepository;

    private readonly ITwoWayDbMapper<EquipmentDb, EquipmentDto> _equipmentMapper;

    public GetEquipmentByMemberQueryHandler(IEquipmentRepository equipmentRepository, ITwoWayDbMapper<EquipmentDb, EquipmentDto> equipmentMapper)
    {
        _equipmentRepository = equipmentRepository;
        _equipmentMapper = equipmentMapper;
    }

    public async Task<IEnumerable<EquipmentDto>> Handle(GetEquipmentByMemberQuery request, CancellationToken cancellationToken)
    {
        var entity = await _equipmentRepository.GetByMemberIdAsync(request.MemberId);
        return entity != null ? new[] { _equipmentMapper.ToDto(entity) } : Array.Empty<EquipmentDto>();
    }
}


