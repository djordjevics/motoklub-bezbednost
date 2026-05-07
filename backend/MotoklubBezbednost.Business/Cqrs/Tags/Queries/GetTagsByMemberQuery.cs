using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Tags.Queries;

public sealed class GetTagsByMemberQuery : IRequest<IEnumerable<TagDto>>
{
    public int MemberId { get; init; }
}
