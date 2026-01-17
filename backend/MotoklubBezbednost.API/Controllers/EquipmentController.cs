using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;
using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.API.Mappers;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EquipmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public EquipmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetEquipment()
    {
        var equipment = await _mediator.Send(new GetAllEquipmentQuery());
        return Ok(equipment);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EquipmentDto>> GetEquipment(int id)
    {
        var request = new GetEquipmentByIdRequest { Id = id };
        var query = request.ToQuery();
        var equipment = await _mediator.Send(query);
        if (equipment == null)
            return NotFound();
        return Ok(equipment);
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetEquipmentByMember(int memberId)
    {
        var request = new GetEquipmentByMemberRequest { MemberId = memberId };
        var query = request.ToQuery();
        var equipment = await _mediator.Send(query);
        return Ok(equipment);
    }

    [HttpPost]
    public async Task<ActionResult<EquipmentDto>> CreateEquipment([FromBody] CreateEquipmentRequest request)
    {
        var command = request.ToCommand();
        var createdEquipment = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetEquipment), new { id = createdEquipment.Id }, createdEquipment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEquipment(int id, [FromBody] UpdateEquipmentRequest request)
    {
        request.Id = id;
        var command = request.ToCommand();
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
