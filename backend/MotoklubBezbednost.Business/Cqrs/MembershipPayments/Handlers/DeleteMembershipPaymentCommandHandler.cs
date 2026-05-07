using MediatR;
using MotoklubBezbednost.Business.Cqrs.MembershipPayments.Commands;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.MembershipPayments.Handlers;

public sealed class DeleteMembershipPaymentCommandHandler : IRequestHandler<DeleteMembershipPaymentCommand, Unit>
{
    private readonly IMembershipPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMembershipPaymentCommandHandler(IMembershipPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteMembershipPaymentCommand request, CancellationToken cancellationToken)
    {
        await _paymentRepository.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
