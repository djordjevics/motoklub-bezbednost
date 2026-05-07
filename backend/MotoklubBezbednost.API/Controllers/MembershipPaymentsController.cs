using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoklubBezbednost.API.Models.Requests;
using MotoklubBezbednost.API.Models.Responses;
using MotoklubBezbednost.Business.Cqrs.MembershipPayments.Commands;
using MotoklubBezbednost.Business.Cqrs.MembershipPayments.Queries;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembershipPaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public MembershipPaymentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<MembershipPaymentResponse>>> GetByMember(int memberId)
    {
        var query = _mapper.Map<GetMembershipPaymentsByMemberQuery>(new GetMembershipPaymentsByMemberRequest { MemberId = memberId });
        var payments = await _mediator.Send(query);
        return Ok(_mapper.Map<IEnumerable<MembershipPaymentResponse>>(payments));
    }

    [HttpPost]
    public async Task<ActionResult<MembershipPaymentResponse>> Create([FromBody] CreateMembershipPaymentRequest request)
    {
        var command = _mapper.Map<CreateMembershipPaymentCommand>(request);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetByMember), new { memberId = request.MemberId }, _mapper.Map<MembershipPaymentResponse>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMembershipPaymentRequest request)
    {
        request.Id = id;
        var command = _mapper.Map<UpdateMembershipPaymentCommand>(request);
        var updated = await _mediator.Send(command);
        if (updated is null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteMembershipPaymentCommand { Id = id });
        return NoContent();
    }
}
