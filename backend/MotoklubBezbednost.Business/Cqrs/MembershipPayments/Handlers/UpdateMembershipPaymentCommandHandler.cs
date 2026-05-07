using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.MembershipPayments.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.MembershipPayments.Handlers;

public sealed class UpdateMembershipPaymentCommandHandler : IRequestHandler<UpdateMembershipPaymentCommand, MembershipPaymentDto?>
{
    private readonly IMembershipPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMembershipPaymentCommandHandler(IMembershipPaymentRepository paymentRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MembershipPaymentDto?> Handle(UpdateMembershipPaymentCommand request, CancellationToken cancellationToken)
    {
        var existing = await _paymentRepository.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(request, existing);
        await _paymentRepository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<MembershipPaymentDto>(existing);
    }
}
