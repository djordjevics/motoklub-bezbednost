using MediatR;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Handlers;

public sealed class UpdateMotorcycleCommandHandler : IRequestHandler<UpdateMotorcycleCommand, MotorcycleDto?>
{
    private readonly IMotorcycleRepository _motorcycleRepository;

    private readonly ITwoWayDbMapper<MotorcycleDb, MotorcycleDto> _motorcycleMapper;

    public UpdateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, ITwoWayDbMapper<MotorcycleDb, MotorcycleDto> motorcycleMapper)
    {
        _motorcycleRepository = motorcycleRepository;
        _motorcycleMapper = motorcycleMapper;
    }

    public async Task<MotorcycleDto?> Handle(UpdateMotorcycleCommand request, CancellationToken cancellationToken)
    {
        var existing = await _motorcycleRepository.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        var dto = _motorcycleMapper.ToDto(existing);

        if (request.BrandName is not null) dto.BrandName = request.BrandName;
        if (request.CommercialName is not null) dto.CommercialName = request.CommercialName;
        if (request.ModelName is not null) dto.ModelName = request.ModelName;
        if (request.EngineDisplacment is not null) dto.EngineDisplacment = request.EngineDisplacment;
        if (request.EnginePower is not null) dto.EnginePower = request.EnginePower;
        if (request.Color is not null) dto.Color = request.Color;
        if (request.RegisterPlate is not null) dto.RegisterPlate = request.RegisterPlate;

        _motorcycleMapper.ToEntity(dto, existing);
        if (request.MemberId is not null) existing.MemberId = request.MemberId.Value;
        existing.LastModificationTimestamp = request.LastModificationTimestamp;

        await _motorcycleRepository.UpdateAsync(existing);
        return _motorcycleMapper.ToDto(existing);
    }
}


