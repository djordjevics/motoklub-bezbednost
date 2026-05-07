using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.MembershipPayments.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.MembershipPayments.Handlers;

public sealed class CreateMembershipPaymentCommandHandler : IRequestHandler<CreateMembershipPaymentCommand, MembershipPaymentDto>
{
    private readonly IMembershipPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMembershipPaymentCommandHandler(IMembershipPaymentRepository paymentRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MembershipPaymentDto> Handle(CreateMembershipPaymentCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<MembershipPaymentDb>(request);
        var created = await _paymentRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<MembershipPaymentDto>(created);
    }
}
