using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoklubBezbednost.API.Models.Responses;
using MotoklubBezbednost.Business.Cqrs.PaymentTypes.Queries;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentTypesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public PaymentTypesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentTypeResponse>>> GetPaymentTypes()
    {
        var paymentTypes = await _mediator.Send(new GetAllPaymentTypesQuery());
        return Ok(_mapper.Map<IEnumerable<PaymentTypeResponse>>(paymentTypes));
    }
}
