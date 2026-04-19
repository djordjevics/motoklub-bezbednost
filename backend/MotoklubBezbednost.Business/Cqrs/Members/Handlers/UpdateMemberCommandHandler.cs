using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Members.Handlers;

public sealed class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, MemberDto?>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public UpdateMemberCommandHandler(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<MemberDto?> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var existing = await _memberRepository.GetByIdWithDetailsAsync(request.Id);
        if (existing == null)
        {
            return null;
        }

        _mapper.Map(request, existing);
        await _memberRepository.UpdateAsync(existing);
        return _mapper.Map<MemberDto>(existing);
    }
}


