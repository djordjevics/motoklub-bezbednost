using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class GetAllEquipmentQueryHandler : IRequestHandler<GetAllEquipmentQuery, IEnumerable<EquipmentDto>>
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;

    public GetAllEquipmentQueryHandler(IEquipmentRepository equipmentRepository, IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EquipmentDto>> Handle(GetAllEquipmentQuery request, CancellationToken cancellationToken)
    {
        var entities = await _equipmentRepository.GetAllWithMemberAsync();
        return entities.Select(e => _mapper.Map<EquipmentDto>(e));
    }
}
