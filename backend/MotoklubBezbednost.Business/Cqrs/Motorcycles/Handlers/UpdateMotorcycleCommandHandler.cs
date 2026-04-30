using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Motorcycles.Handlers;

public sealed class UpdateMotorcycleCommandHandler : IRequestHandler<UpdateMotorcycleCommand, MotorcycleDto?>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _motorcycleRepository = motorcycleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MotorcycleDto?> Handle(UpdateMotorcycleCommand request, CancellationToken cancellationToken)
    {
        var existing = await _motorcycleRepository.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(request, existing);
        if (request.MemberId is not null) existing.MemberId = request.MemberId.Value;
        await _motorcycleRepository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<MotorcycleDto>(existing);
    }
}


