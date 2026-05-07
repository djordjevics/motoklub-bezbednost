using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.MembershipPayments.Queries;

public sealed class GetMembershipPaymentsByMemberQuery : IRequest<IEnumerable<MembershipPaymentDto>>
{
    public int MemberId { get; init; }
}
