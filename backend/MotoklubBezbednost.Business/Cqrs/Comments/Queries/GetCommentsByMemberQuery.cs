using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Comments.Queries;

public sealed class GetCommentsByMemberQuery : IRequest<IEnumerable<CommentDto>>
{
    public int MemberId { get; init; }
}
