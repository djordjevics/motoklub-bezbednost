using MediatR;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Handlers;

public sealed class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand, MotorcycleDto>
{
    private readonly IMotorcycleRepository _motorcycleRepository;

    private readonly ITwoWayDbMapper<MotorcycleDb, MotorcycleDto> _motorcycleMapper;

    public CreateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, ITwoWayDbMapper<MotorcycleDb, MotorcycleDto> motorcycleMapper)
    {
        _motorcycleRepository = motorcycleRepository;
        _motorcycleMapper = motorcycleMapper;
    }

    public async Task<MotorcycleDto> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
    {
        var dto = new MotorcycleDto
        {
            BrandName = request.BrandName,
            CommercialName = request.CommercialName,
            ModelName = request.ModelName,
            EngineDisplacment = request.EngineDisplacment,
            EnginePower = request.EnginePower,
            Color = request.Color,
            RegisterPlate = request.RegisterPlate
        };

        var entity = _motorcycleMapper.ToEntity(dto);
        entity.CreationTimestamp = request.CreationTimestamp;
        entity.MemberId = request.MemberId;

        var created = await _motorcycleRepository.AddAsync(entity);
        return _motorcycleMapper.ToDto(created);
    }
}


