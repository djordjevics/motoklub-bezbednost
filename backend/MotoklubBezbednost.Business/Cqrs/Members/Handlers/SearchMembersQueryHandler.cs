using MediatR;
using MotoklubBezbednost.Business.Cqrs.Members.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Members.Handlers;

public sealed class SearchMembersQueryHandler : IRequestHandler<SearchMembersQuery, IEnumerable<MemberDto>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ITwoWayDbMapper<MemberDb, MemberDto> _memberMapper;

    public SearchMembersQueryHandler(IMemberRepository memberRepository, ITwoWayDbMapper<MemberDb, MemberDto> memberMapper)
    {
        _memberRepository = memberRepository;
        _memberMapper = memberMapper;
    }

    public async Task<IEnumerable<MemberDto>> Handle(SearchMembersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _memberRepository.SearchAsync(request.Query);
        return _memberMapper.ToDto(entities);
    }
}


