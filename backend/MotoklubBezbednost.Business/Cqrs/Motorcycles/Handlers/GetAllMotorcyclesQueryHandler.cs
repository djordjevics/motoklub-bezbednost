using MediatR;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;
using MotoklubBezbednost.Business.Dtos;
using AutoMapper;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Handlers;

public sealed class GetAllMotorcyclesQueryHandler : IRequestHandler<GetAllMotorcyclesQuery, IEnumerable<MotorcycleDto>>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IMapper _mapper;

    public GetAllMotorcyclesQueryHandler(IMotorcycleRepository motorcycleRepository, IMapper mapper)
    {
        _motorcycleRepository = motorcycleRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MotorcycleDto>> Handle(GetAllMotorcyclesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _motorcycleRepository.GetAllAsync();
        return entities.Select(e => _mapper.Map<MotorcycleDto>(e));
    }
}


