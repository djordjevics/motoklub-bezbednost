using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoklubBezbednost.API.Models.Requests;
using MotoklubBezbednost.API.Models.Responses;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipmentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public EquipmentController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EquipmentResponse>>> GetEquipment()
    {
        var equipment = await _mediator.Send(new GetAllEquipmentQuery());
        return Ok(_mapper.Map<IEnumerable<EquipmentResponse>>(equipment));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EquipmentResponse>> GetEquipment(int id)
    {
        var query = _mapper.Map<GetEquipmentByIdQuery>(new GetEquipmentByIdRequest { Id = id });
        var equipment = await _mediator.Send(query);
        if (equipment == null)
            return NotFound();
        return Ok(_mapper.Map<EquipmentResponse>(equipment));
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<EquipmentResponse>>> GetEquipmentByMember(int memberId)
    {
        var query = _mapper.Map<GetEquipmentByMemberQuery>(new GetEquipmentByMemberRequest { MemberId = memberId });
        var equipment = await _mediator.Send(query);
        return Ok(_mapper.Map<IEnumerable<EquipmentResponse>>(equipment));
    }

    [HttpPost]
    public async Task<ActionResult<EquipmentResponse>> CreateEquipment([FromBody] CreateEquipmentRequest request)
    {
        var command = _mapper.Map<CreateEquipmentCommand>(request);
        var createdEquipment = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetEquipment), new { id = createdEquipment.Id }, _mapper.Map<EquipmentResponse>(createdEquipment));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEquipment(int id, [FromBody] UpdateEquipmentRequest request)
    {
        request.Id = id;
        var command = _mapper.Map<UpdateEquipmentCommand>(request);
        var updated = await _mediator.Send(command);
        if (updated is null)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipment(int id)
    {
        await _mediator.Send(new DeleteEquipmentCommand { Id = id });
        return NoContent();
    }
}
