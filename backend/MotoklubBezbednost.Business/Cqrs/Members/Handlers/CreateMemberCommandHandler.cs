using MediatR;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Members.Handlers;

public sealed class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, MemberDto>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ITwoWayDbMapper<MemberDb, MemberDto> _memberMapper;

    public CreateMemberCommandHandler(IMemberRepository memberRepository, ITwoWayDbMapper<MemberDb, MemberDto> memberMapper)
    {
        _memberRepository = memberRepository;
        _memberMapper = memberMapper;
    }

    public async Task<MemberDto> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var dto = new MemberDto
        {
            Name = request.Name,
            Surname = request.Surname,
            Jmbg = request.Jmbg,
            DateOfBirth = request.DateOfBirth,
            Workplace = request.Workplace,
            MobilePhone = request.MobilePhone,
            EmergencyContact = request.EmergencyContact,
            EmergencyContactPhone = request.EmergencyContactPhone,
            Email = request.Email,
            Address = request.Address,
            Note = request.Note,
            RegisteredOn = request.CreationTimestamp
        };

        var entity = _memberMapper.ToEntity(dto);
        entity.CreationTimestamp = request.CreationTimestamp;

        var created = await _memberRepository.AddAsync(entity);
        return _memberMapper.ToDto(created);
    }
}


