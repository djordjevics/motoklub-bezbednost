using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Handlers;

public sealed class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand, MotorcycleDto>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IMapper _mapper;

    public CreateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, IMapper mapper)
    {
        _motorcycleRepository = motorcycleRepository;
        _mapper = mapper;
    }

    public async Task<MotorcycleDto> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<MotorcycleDb>(request);
        var created = await _motorcycleRepository.AddAsync(entity);
        return _mapper.Map<MotorcycleDto>(created);
    }
}


