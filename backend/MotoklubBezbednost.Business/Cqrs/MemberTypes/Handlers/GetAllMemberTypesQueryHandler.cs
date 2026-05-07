using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.MemberTypes.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.MemberTypes.Handlers;

public sealed class GetAllMemberTypesQueryHandler : IRequestHandler<GetAllMemberTypesQuery, IEnumerable<MemberTypeDto>>
{
    private readonly IMemberTypeRepository _repository;
    private readonly IMapper _mapper;

    public GetAllMemberTypesQueryHandler(IMemberTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MemberTypeDto>> Handle(GetAllMemberTypesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => _mapper.Map<MemberTypeDto>(e));
    }
}
