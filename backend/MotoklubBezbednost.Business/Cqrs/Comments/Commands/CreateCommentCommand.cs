using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Comments.Commands;

public sealed class CreateCommentCommand : IRequest<CommentDto>
{
    public int MemberId { get; init; }
    public string? CommentText { get; init; }
}
