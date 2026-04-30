using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class CreateEquipmentCommandHandler : IRequestHandler<CreateEquipmentCommand, EquipmentDto>
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateEquipmentCommandHandler(IEquipmentRepository equipmentRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EquipmentDto> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<EquipmentDb>(request);
        var created = await _equipmentRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<EquipmentDto>(created);
    }
}


