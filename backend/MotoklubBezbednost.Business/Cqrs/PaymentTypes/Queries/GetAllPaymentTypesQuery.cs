using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.PaymentTypes.Queries;

public sealed class GetAllPaymentTypesQuery : IRequest<IEnumerable<PaymentTypeDto>>
{
}
