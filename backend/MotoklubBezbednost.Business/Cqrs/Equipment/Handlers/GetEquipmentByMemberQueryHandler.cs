using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class GetEquipmentByMemberQueryHandler : IRequestHandler<GetEquipmentByMemberQuery, IEnumerable<EquipmentDto>>
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;

    public GetEquipmentByMemberQueryHandler(IEquipmentRepository equipmentRepository, IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EquipmentDto>> Handle(GetEquipmentByMemberQuery request, CancellationToken cancellationToken)
    {
        var entity = await _equipmentRepository.GetByMemberIdAsync(request.MemberId);
        return entity != null ? new[] { _mapper.Map<EquipmentDto>(entity) } : Array.Empty<EquipmentDto>();
    }
}
