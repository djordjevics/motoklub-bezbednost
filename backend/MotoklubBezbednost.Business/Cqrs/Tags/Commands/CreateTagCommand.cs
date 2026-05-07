using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Tags.Commands;

public sealed class CreateTagCommand : IRequest<TagDto>
{
    public int MemberId { get; init; }
    public int? TagNumber { get; init; }
    public DateTime? AssignedDate { get; init; }
    public DateTime? ValidFrom { get; init; }
    public DateTime? ValidTo { get; init; }
}
