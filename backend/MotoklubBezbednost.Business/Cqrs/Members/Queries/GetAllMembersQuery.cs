using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Members.Queries;

public sealed class GetAllMembersQuery : IRequest<IEnumerable<MemberDto>>
{
}


