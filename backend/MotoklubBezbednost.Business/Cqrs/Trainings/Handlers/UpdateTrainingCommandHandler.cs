using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class UpdateTrainingCommandHandler : IRequestHandler<UpdateTrainingCommand, TrainingDto?>
{
    private readonly ITrainingRepository _trainingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTrainingCommandHandler(ITrainingRepository trainingRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _trainingRepository = trainingRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TrainingDto?> Handle(UpdateTrainingCommand request, CancellationToken cancellationToken)
    {
        var existing = await _trainingRepository.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(request, existing);
        await _trainingRepository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TrainingDto>(existing);
    }
}
