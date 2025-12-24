using MediatR;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Handlers;

public sealed class GetMotorcycleByIdQueryHandler : IRequestHandler<GetMotorcycleByIdQuery, MotorcycleDto?>
{
    private readonly IMotorcycleRepository _motorcycleRepository;

    private readonly ITwoWayDbMapper<MotorcycleDb, MotorcycleDto> _motorcycleMapper;

    public GetMotorcycleByIdQueryHandler(IMotorcycleRepository motorcycleRepository, ITwoWayDbMapper<MotorcycleDb, MotorcycleDto> motorcycleMapper)
    {
        _motorcycleRepository = motorcycleRepository;
        _motorcycleMapper = motorcycleMapper;
    }

    public async Task<MotorcycleDto?> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _motorcycleRepository.GetByIdAsync(request.Id);
        return entity is null ? null : _motorcycleMapper.ToDto(entity);
    }
}


