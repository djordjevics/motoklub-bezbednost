using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Members.Commands;

public sealed class UpdateMemberCommand : IRequest<MemberDto?>
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Surname { get; init; }
    public string? Jmbg { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public string? Workplace { get; init; }
    public string? MobilePhone { get; init; }
    public string? EmergencyContact { get; init; }
    public string? EmergencyContactPhone { get; init; }
    public string? Email { get; init; }
    public string? Address { get; init; }
    public string? Note { get; init; }
    public DateTime LastModificationTimestamp { get; init; }
}


