using MediatR;
using MotoklubBezbednost.Business.Cqrs.Members.Queries;
using MotoklubBezbednost.Business.Dtos;
using AutoMapper;
using MotoklubBezbednost.Business.Rules;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Members.Handlers;

public sealed class SearchMembersQueryHandler : IRequestHandler<SearchMembersQuery, IEnumerable<MemberDto>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public SearchMembersQueryHandler(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MemberDto>> Handle(SearchMembersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _memberRepository.SearchAsync(request.Query);
        var today = DateTime.Today;
        return entities.Select(e =>
        {
            var dto = _mapper.Map<MemberDto>(e);
            MembershipRules.EnrichMembershipExempt(e, dto, today);
            return dto;
        });
    }
}
