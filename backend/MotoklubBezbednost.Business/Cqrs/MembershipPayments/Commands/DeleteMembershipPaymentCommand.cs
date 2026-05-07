using MediatR;

namespace MotoklubBezbednost.Business.Cqrs.MembershipPayments.Commands;

public sealed class DeleteMembershipPaymentCommand : IRequest<Unit>
{
    public int Id { get; init; }
}
