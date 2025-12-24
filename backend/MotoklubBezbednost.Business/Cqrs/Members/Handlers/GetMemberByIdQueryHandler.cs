using MediatR;
using MotoklubBezbednost.Business.Cqrs.Members.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Members.Handlers;

public sealed class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, MemberDto?>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ITwoWayDbMapper<MemberDb, MemberDto> _memberMapper;

    public GetMemberByIdQueryHandler(IMemberRepository memberRepository, ITwoWayDbMapper<MemberDb, MemberDto> memberMapper)
    {
        _memberRepository = memberRepository;
        _memberMapper = memberMapper;
    }

    public async Task<MemberDto?> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _memberRepository.GetByIdWithDetailsAsync(request.Id);
        return entity is null ? null : _memberMapper.ToDto(entity);
    }
}


