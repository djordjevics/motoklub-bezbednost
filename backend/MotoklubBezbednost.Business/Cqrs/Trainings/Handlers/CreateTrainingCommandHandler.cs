using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class CreateTrainingCommandHandler : IRequestHandler<CreateTrainingCommand, TrainingDto>
{
    private readonly ITrainingRepository _trainingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTrainingCommandHandler(ITrainingRepository trainingRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _trainingRepository = trainingRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TrainingDto> Handle(CreateTrainingCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<TrainingDb>(request);
        var created = await _trainingRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TrainingDto>(created);
    }
}


