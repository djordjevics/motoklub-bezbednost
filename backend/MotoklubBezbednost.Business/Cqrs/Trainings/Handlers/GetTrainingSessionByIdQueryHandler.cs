using MediatR;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;
using MotoklubBezbednost.Business.Dtos;
using AutoMapper;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Trainings.Handlers;

public sealed class GetTrainingSessionByIdQueryHandler : IRequestHandler<GetTrainingSessionByIdQuery, TrainingSessionDto?>
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly IMapper _mapper;

    public GetTrainingSessionByIdQueryHandler(ITrainingSessionRepository trainingSessionRepository, IMapper mapper)
    {
        _trainingSessionRepository = trainingSessionRepository;
        _mapper = mapper;
    }

    public async Task<TrainingSessionDto?> Handle(GetTrainingSessionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _trainingSessionRepository.GetByIdWithDetailsAsync(request.Id);
        return entity is null ? null : _mapper.Map<TrainingSessionDto>(entity);
    }
}


