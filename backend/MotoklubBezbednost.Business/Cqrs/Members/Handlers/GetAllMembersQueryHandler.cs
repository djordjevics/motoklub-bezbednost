using MediatR;
using MotoklubBezbednost.Business.Cqrs.Members.Queries;
using MotoklubBezbednost.Business.Dtos;
using AutoMapper;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Members.Handlers;

public sealed class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, IEnumerable<MemberDto>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public GetAllMembersQueryHandler(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MemberDto>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _memberRepository.GetAllWithDetailsAsync();
        return entities.Select(e => _mapper.Map<MemberDto>(e));
    }
}


