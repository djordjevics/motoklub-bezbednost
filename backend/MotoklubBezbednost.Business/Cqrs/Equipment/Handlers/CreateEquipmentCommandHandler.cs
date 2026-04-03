using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class CreateEquipmentCommandHandler : IRequestHandler<CreateEquipmentCommand, EquipmentDto>
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;

    public CreateEquipmentCommandHandler(IEquipmentRepository equipmentRepository, IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
        _mapper = mapper;
    }

    public async Task<EquipmentDto> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<EquipmentDb>(request);
        var created = await _equipmentRepository.AddAsync(entity);
        return _mapper.Map<EquipmentDto>(created);
    }
}


