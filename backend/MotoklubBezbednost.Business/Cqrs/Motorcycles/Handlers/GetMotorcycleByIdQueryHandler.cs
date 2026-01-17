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
    private readonly IMapper _mapper;

    public GetMotorcycleByIdQueryHandler(IMotorcycleRepository motorcycleRepository, IMapper mapper)
    {
        _motorcycleRepository = motorcycleRepository;
        _mapper = mapper;
    }

    public async Task<MotorcycleDto?> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _motorcycleRepository.GetByIdAsync(request.Id);
        return entity is null ? null : _mapper.Map<MotorcycleDb, MotorcycleDto>(entity);
    }
}


