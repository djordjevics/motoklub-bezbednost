using MediatR;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Handlers;

public sealed class GetMotorcyclesByMemberQueryHandler : IRequestHandler<GetMotorcyclesByMemberQuery, IEnumerable<MotorcycleDto>>
{
    private readonly IMotorcycleRepository _motorcycleRepository;

    private readonly ITwoWayDbMapper<MotorcycleDb, MotorcycleDto> _motorcycleMapper;

    public GetMotorcyclesByMemberQueryHandler(IMotorcycleRepository motorcycleRepository, ITwoWayDbMapper<MotorcycleDb, MotorcycleDto> motorcycleMapper)
    {
        _motorcycleRepository = motorcycleRepository;
        _motorcycleMapper = motorcycleMapper;
    }

    public async Task<IEnumerable<MotorcycleDto>> Handle(GetMotorcyclesByMemberQuery request, CancellationToken cancellationToken)
    {
        var entities = await _motorcycleRepository.GetByMemberIdAsync(request.MemberId);
        return _motorcycleMapper.ToDto(entities);
    }
}


