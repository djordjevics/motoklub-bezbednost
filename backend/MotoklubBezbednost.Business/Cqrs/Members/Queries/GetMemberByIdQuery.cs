using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Members.Queries;

public sealed class GetMemberByIdQuery : IRequest<MemberDto?>
{
    public int Id { get; init; }
}


