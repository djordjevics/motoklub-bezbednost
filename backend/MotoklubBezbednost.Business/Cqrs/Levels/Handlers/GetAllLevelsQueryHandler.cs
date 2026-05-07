using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Levels.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Levels.Handlers;

public sealed class GetAllLevelsQueryHandler : IRequestHandler<GetAllLevelsQuery, IEnumerable<LevelDto>>
{
    private readonly ILevelRepository _repository;
    private readonly IMapper _mapper;

    public GetAllLevelsQueryHandler(ILevelRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LevelDto>> Handle(GetAllLevelsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => _mapper.Map<LevelDto>(e));
    }
}
