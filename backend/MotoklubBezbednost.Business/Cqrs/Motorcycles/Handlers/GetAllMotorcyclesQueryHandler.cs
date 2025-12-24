using MediatR;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Handlers;

public sealed class GetAllMotorcyclesQueryHandler : IRequestHandler<GetAllMotorcyclesQuery, IEnumerable<MotorcycleDto>>
{
    private readonly IMotorcycleRepository _motorcycleRepository;

    private readonly ITwoWayDbMapper<MotorcycleDb, MotorcycleDto> _motorcycleMapper;

    public GetAllMotorcyclesQueryHandler(IMotorcycleRepository motorcycleRepository, ITwoWayDbMapper<MotorcycleDb, MotorcycleDto> motorcycleMapper)
    {
        _motorcycleRepository = motorcycleRepository;
        _motorcycleMapper = motorcycleMapper;
    }

    public async Task<IEnumerable<MotorcycleDto>> Handle(GetAllMotorcyclesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _motorcycleRepository.GetAllWithMemberAsync();
        return _motorcycleMapper.ToDto(entities);
    }
}


