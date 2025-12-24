using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MotorcyclesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public MotorcyclesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcycles([FromQuery] GetAllMotorcyclesRequest request)
    {
        var query = _mapper.Map<GetAllMotorcyclesRequest, GetAllMotorcyclesQuery>(request);
        var motorcycles = await _mediator.Send(query);
        return Ok(motorcycles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MotorcycleDto>> GetMotorcycle([FromRoute] GetMotorcycleByIdRequest request)
    {
        var query = _mapper.Map<GetMotorcycleByIdRequest, GetMotorcycleByIdQuery>(request);
        var motorcycle = await _mediator.Send(query);
        if (motorcycle == null)
            return NotFound();
        return Ok(motorcycle);
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByMember([FromRoute] GetMotorcyclesByMemberRequest request)
    {
        var query = _mapper.Map<GetMotorcyclesByMemberRequest, GetMotorcyclesByMemberQuery>(request);
        var motorcycles = await _mediator.Send(query);
        return Ok(motorcycles);
    }

    [HttpPost]
    public async Task<ActionResult<MotorcycleDto>> CreateMotorcycle([FromBody] CreateMotorcycleRequest request)
    {
        var cmd = _mapper.Map<CreateMotorcycleRequest, CreateMotorcycleCommand>(request);
        var createdMotorcycle = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetMotorcycle), new { id = createdMotorcycle.Id }, createdMotorcycle);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMotorcycle([FromRoute] int id, [FromBody] UpdateMotorcycleRequest request)
    {
        if (id != request.Id)
            return BadRequest();

        var cmd = _mapper.Map<UpdateMotorcycleRequest, UpdateMotorcycleCommand>(request);
        var updated = await _mediator.Send(cmd);
        if (updated is null)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMotorcycle([FromRoute] DeleteMotorcycleRequest request)
    {
        var cmd = _mapper.Map<DeleteMotorcycleRequest, DeleteMotorcycleCommand>(request);
        await _mediator.Send(cmd);
        return NoContent();
    }
}
