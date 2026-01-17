using MediatR;

namespace MotoklubBezbednost.Business.Cqrs.Members.Commands;

public sealed class DeleteMemberCommand : IRequest
{
    public int Id { get; init; }
}


