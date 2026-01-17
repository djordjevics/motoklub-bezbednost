using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Members.Commands;

public sealed class CreateMemberCommand : IRequest<MemberDto>
{
    public string Name { get; init; } = null!;
    public string Surname { get; init; } = null!;
    public string? Jmbg { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public string? Workplace { get; init; }
    public string MobilePhone { get; init; } = null!;
    public string? EmergencyContact { get; init; }
    public string? EmergencyContactPhone { get; init; }
    public string Email { get; init; } = null!;
    public string Address { get; init; } = null!;
    public string? Note { get; init; }
}


