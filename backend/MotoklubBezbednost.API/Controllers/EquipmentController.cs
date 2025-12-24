using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetEquipment([FromQuery] GetAllEquipmentRequest request)
    {
        var query = _mapper.Map<GetAllEquipmentRequest, GetAllEquipmentQuery>(request);
        var equipment = await _mediator.Send(query);
        return Ok(equipment);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EquipmentDto>> GetEquipment([FromRoute] GetEquipmentByIdRequest request)
    {
        var query = _mapper.Map<GetEquipmentByIdRequest, GetEquipmentByIdQuery>(request);
        var equipment = await _mediator.Send(query);
        if (equipment == null)
            return NotFound();
        return Ok(equipment);
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetEquipmentByMember([FromRoute] GetEquipmentByMemberRequest request)
    {
        var query = _mapper.Map<GetEquipmentByMemberRequest, GetEquipmentByMemberQuery>(request);
        var equipment = await _mediator.Send(query);
        return Ok(equipment);
    }

    [HttpPost]
    public async Task<ActionResult<EquipmentDto>> CreateEquipment([FromBody] CreateEquipmentRequest request)
    {
        var cmd = _mapper.Map<CreateEquipmentRequest, CreateEquipmentCommand>(request);
        var createdEquipment = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetEquipment), new { id = createdEquipment.Id }, createdEquipment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEquipment([FromRoute] int id, [FromBody] UpdateEquipmentRequest request)
    {
        if (id != request.Id)
            return BadRequest();

        var cmd = _mapper.Map<UpdateEquipmentRequest, UpdateEquipmentCommand>(request);
        var updated = await _mediator.Send(cmd);
        if (updated is null)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipment([FromRoute] DeleteEquipmentRequest request)
    {
        var cmd = _mapper.Map<DeleteEquipmentRequest, DeleteEquipmentCommand>(request);
        await _mediator.Send(cmd);
        return NoContent();
    }
}
