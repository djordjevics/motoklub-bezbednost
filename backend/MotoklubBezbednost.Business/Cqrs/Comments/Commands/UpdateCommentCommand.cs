using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Comments.Commands;

public sealed class UpdateCommentCommand : IRequest<CommentDto?>
{
    public int Id { get; init; }
    public string? CommentText { get; init; }
}
