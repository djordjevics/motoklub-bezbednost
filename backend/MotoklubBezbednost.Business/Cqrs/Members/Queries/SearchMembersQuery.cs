using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Members.Queries;

public sealed class SearchMembersQuery : IRequest<IEnumerable<MemberDto>>
{
    public string Query { get; init; } = string.Empty;
}


