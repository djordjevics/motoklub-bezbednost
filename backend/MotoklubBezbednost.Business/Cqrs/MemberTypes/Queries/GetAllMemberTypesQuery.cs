using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.MemberTypes.Queries;

public sealed class GetAllMemberTypesQuery : IRequest<IEnumerable<MemberTypeDto>>
{
}
