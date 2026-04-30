using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class CreateTrainingSessionCommandHandler : IRequestHandler<CreateTrainingSessionCommand, TrainingSessionDto>
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTrainingSessionCommandHandler(ITrainingSessionRepository trainingSessionRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _trainingSessionRepository = trainingSessionRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TrainingSessionDto> Handle(CreateTrainingSessionCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<TrainingSessionDb>(request);
        var created = await _trainingSessionRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TrainingSessionDto>(created);
    }
}


