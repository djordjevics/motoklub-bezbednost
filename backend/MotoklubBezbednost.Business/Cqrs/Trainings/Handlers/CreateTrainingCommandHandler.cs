using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class CreateTrainingCommandHandler : IRequestHandler<CreateTrainingCommand, TrainingDto>
{
    private readonly ITrainingRepository _trainingRepository;

    private readonly ITwoWayDbMapper<TrainingDb, TrainingDto> _trainingMapper;

    public CreateTrainingCommandHandler(ITrainingRepository trainingRepository, ITwoWayDbMapper<TrainingDb, TrainingDto> trainingMapper)
    {
        _trainingRepository = trainingRepository;
        _trainingMapper = trainingMapper;
    }

    public async Task<TrainingDto> Handle(CreateTrainingCommand request, CancellationToken cancellationToken)
    {
        var dto = new TrainingDto
        {
            IsCertificateIssued = request.IsCertificateIssued,
            Note = request.Note
        };

        var entity = _trainingMapper.ToEntity(dto);
        entity.CreationTimestamp = request.CreationTimestamp;
        entity.MemberId = request.MemberId;
        entity.MotorcycleId = request.MotorcycleId;
        entity.TrainingSessionId = request.TrainingSessionId;

        var created = await _trainingRepository.AddAsync(entity);
        return _trainingMapper.ToDto(created);
    }
}


