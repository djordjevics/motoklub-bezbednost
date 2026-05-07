using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Tags.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Tags.Handlers;

public sealed class GetTagsByMemberQueryHandler : IRequestHandler<GetTagsByMemberQuery, IEnumerable<TagDto>>
{
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;

    public GetTagsByMemberQueryHandler(ITagRepository tagRepository, IMapper mapper)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TagDto>> Handle(GetTagsByMemberQuery request, CancellationToken cancellationToken)
    {
        var entities = await _tagRepository.FindAsync(t => t.MemberId == request.MemberId);
        return entities.Select(e => _mapper.Map<TagDto>(e));
    }
}
