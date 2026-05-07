using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.PaymentTypes.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.PaymentTypes.Handlers;

public sealed class GetAllPaymentTypesQueryHandler : IRequestHandler<GetAllPaymentTypesQuery, IEnumerable<PaymentTypeDto>>
{
    private readonly IPaymentTypeRepository _repository;
    private readonly IMapper _mapper;

    public GetAllPaymentTypesQueryHandler(IPaymentTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PaymentTypeDto>> Handle(GetAllPaymentTypesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => _mapper.Map<PaymentTypeDto>(e));
    }
}
