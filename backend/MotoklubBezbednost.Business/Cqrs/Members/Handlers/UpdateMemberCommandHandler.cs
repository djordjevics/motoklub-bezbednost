using MediatR;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Members.Handlers;

public sealed class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, MemberDto?>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ITwoWayDbMapper<MemberDb, MemberDto> _memberMapper;

    public UpdateMemberCommandHandler(IMemberRepository memberRepository, ITwoWayDbMapper<MemberDb, MemberDto> memberMapper)
    {
        _memberRepository = memberRepository;
        _memberMapper = memberMapper;
    }

    public async Task<MemberDto?> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var existing = await _memberRepository.GetByIdWithDetailsAsync(request.Id);
        if (existing == null)
        {
            return null;
        }

        // Apply partial updates
        if (request.Name is not null) existing.Name = request.Name;
        if (request.Surname is not null) existing.Surname = request.Surname;
        if (request.Jmbg is not null) existing.Jmbg = request.Jmbg;
        if (request.DateOfBirth is not null) existing.DateOfBirth = request.DateOfBirth;
        if (request.Workplace is not null) existing.Workplace = request.Workplace;
        if (request.MobilePhone is not null) existing.MobilePhone = request.MobilePhone;
        if (request.EmergencyContact is not null) existing.EmergencyContact = request.EmergencyContact;
        if (request.EmergencyContactPhone is not null) existing.EmergencyContactPhone = request.EmergencyContactPhone;
        if (request.Email is not null) existing.Email = request.Email;
        if (request.Address is not null) existing.Address = request.Address;
        if (request.Note is not null) existing.Note = request.Note;

        existing.LastModificationTimestamp = request.LastModificationTimestamp;

        await _memberRepository.UpdateAsync(existing);
        return _memberMapper.ToDto(existing);
    }
}


