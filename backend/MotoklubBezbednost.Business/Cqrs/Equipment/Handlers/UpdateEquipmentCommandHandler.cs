using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class UpdateEquipmentCommandHandler : IRequestHandler<UpdateEquipmentCommand, EquipmentDto?>
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateEquipmentCommandHandler(IEquipmentRepository equipmentRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EquipmentDto?> Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var existing = await _equipmentRepository.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(request, existing);
        await _equipmentRepository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<EquipmentDto>(existing);
    }
}


