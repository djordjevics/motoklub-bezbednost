using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.MembershipPayments.Commands;

public sealed class CreateMembershipPaymentCommand : IRequest<MembershipPaymentDto>
{
    public int MemberId { get; init; }
    public int? Amount { get; init; }
    public DateTime? PaymentDate { get; init; }
    public int? PaymentForYear { get; init; }
    public int? PaymentTypeId { get; init; }
    public string? Note { get; init; }
}
