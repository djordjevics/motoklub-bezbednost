using MediatR;

namespace MotoklubBezbednost.Business.Cqrs.Tags.Commands;

public sealed class DeleteTagCommand : IRequest<Unit>
{
    public int Id { get; init; }
}
