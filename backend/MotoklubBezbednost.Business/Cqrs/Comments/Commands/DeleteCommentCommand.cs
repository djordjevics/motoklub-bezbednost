using MediatR;

namespace MotoklubBezbednost.Business.Cqrs.Comments.Commands;

public sealed class DeleteCommentCommand : IRequest<Unit>
{
    public int Id { get; init; }
}
