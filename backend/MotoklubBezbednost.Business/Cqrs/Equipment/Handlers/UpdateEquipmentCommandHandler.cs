using MediatR;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Equipment.Handlers;

public sealed class UpdateEquipmentCommandHandler : IRequestHandler<UpdateEquipmentCommand, EquipmentDto?>
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;

    public UpdateEquipmentCommandHandler(IEquipmentRepository equipmentRepository, IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
        _mapper = mapper;
    }

    public async Task<EquipmentDto?> Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var existing = await _equipmentRepository.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        request.ApplyTo(existing);
        await _equipmentRepository.UpdateAsync(existing);
        return _mapper.Map<Data.Models.EquipmentDb, EquipmentDto>(existing);
    }
}


