using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoklubBezbednost.API.Models.Requests;
using MotoklubBezbednost.API.Models.Responses;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<ActionResult<IEnumerable<MotorcycleResponse>>> GetMotorcycles()
    {
        var motorcycles = await _mediator.Send(new GetAllMotorcyclesQuery());
        return Ok(_mapper.Map<IEnumerable<MotorcycleResponse>>(motorcycles));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MotorcycleResponse>> GetMotorcycle(int id)
    {
        var query = _mapper.Map<GetMotorcycleByIdQuery>(new GetMotorcycleByIdRequest { Id = id });
        var motorcycle = await _mediator.Send(query);
        if (motorcycle == null)
            return NotFound();
        return Ok(_mapper.Map<MotorcycleResponse>(motorcycle));
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<MotorcycleResponse>>> GetMotorcyclesByMember(int memberId)
    {
        var query = _mapper.Map<GetMotorcyclesByMemberQuery>(new GetMotorcyclesByMemberRequest { MemberId = memberId });
        var motorcycles = await _mediator.Send(query);
        return Ok(_mapper.Map<IEnumerable<MotorcycleResponse>>(motorcycles));
    }

    [HttpPost]
    public async Task<ActionResult<MotorcycleResponse>> CreateMotorcycle([FromBody] CreateMotorcycleRequest request)
    {
        var command = _mapper.Map<CreateMotorcycleCommand>(request);
        var createdMotorcycle = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetMotorcycle), new { id = createdMotorcycle.Id }, _mapper.Map<MotorcycleResponse>(createdMotorcycle));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMotorcycle(int id, [FromBody] UpdateMotorcycleRequest request)
    {
        request.Id = id;
        var command = _mapper.Map<UpdateMotorcycleCommand>(request);
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
