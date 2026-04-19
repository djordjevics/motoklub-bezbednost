using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class GetEquipmentByIdQueryHandler : IRequestHandler<GetEquipmentByIdQuery, EquipmentDto?>
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;

    public GetEquipmentByIdQueryHandler(IEquipmentRepository equipmentRepository, IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
        _mapper = mapper;
    }

    public async Task<EquipmentDto?> Handle(GetEquipmentByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _equipmentRepository.GetByIdAsync(request.Id);
        return entity is null ? null : _mapper.Map<EquipmentDto>(entity);
    }
}
