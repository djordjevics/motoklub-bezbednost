using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;
using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.API.Mappers;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MotorcyclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MotorcyclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcycles()
    {
        var motorcycles = await _mediator.Send(new GetAllMotorcyclesQuery());
        return Ok(motorcycles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MotorcycleDto>> GetMotorcycle(int id)
    {
        var request = new GetMotorcycleByIdRequest { Id = id };
        var query = request.ToQuery();
        var motorcycle = await _mediator.Send(query);
        if (motorcycle == null)
            return NotFound();
        return Ok(motorcycle);
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByMember(int memberId)
    {
        var request = new GetMotorcyclesByMemberRequest { MemberId = memberId };
        var query = request.ToQuery();
        var motorcycles = await _mediator.Send(query);
        return Ok(motorcycles);
    }

    [HttpPost]
    public async Task<ActionResult<MotorcycleDto>> CreateMotorcycle([FromBody] CreateMotorcycleRequest request)
    {
        var command = request.ToCommand();
        var createdMotorcycle = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetMotorcycle), new { id = createdMotorcycle.Id }, createdMotorcycle);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMotorcycle(int id, [FromBody] UpdateMotorcycleRequest request)
    {
        request.Id = id;
        var command = request.ToCommand();
        var updated = await _mediator.Send(command);
        if (updated is null)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMotorcycle(int id)
    {
        await _mediator.Send(new DeleteMotorcycleCommand { Id = id });
        return NoContent();
    }
}
