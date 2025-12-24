using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrainingsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TrainingsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainingSessionDto>>> GetTrainingSessions([FromQuery] GetAllTrainingSessionsRequest request)
    {
        var query = _mapper.Map<GetAllTrainingSessionsRequest, GetAllTrainingSessionsQuery>(request);
        var sessions = await _mediator.Send(query);
        return Ok(sessions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrainingSessionDto>> GetTrainingSession([FromRoute] GetTrainingSessionByIdRequest request)
    {
        var query = _mapper.Map<GetTrainingSessionByIdRequest, GetTrainingSessionByIdQuery>(request);
        var session = await _mediator.Send(query);
        if (session == null)
            return NotFound();
        return Ok(session);
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<TrainingDto>>> GetTrainingsByMember([FromRoute] GetTrainingsByMemberRequest request)
    {
        var query = _mapper.Map<GetTrainingsByMemberRequest, GetTrainingsByMemberQuery>(request);
        var trainings = await _mediator.Send(query);
        return Ok(trainings);
    }

    [HttpPost("sessions")]
    public async Task<ActionResult<TrainingSessionDto>> CreateTrainingSession([FromBody] CreateTrainingSessionRequest request)
    {
        var cmd = _mapper.Map<CreateTrainingSessionRequest, CreateTrainingSessionCommand>(request);
        var createdSession = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetTrainingSession), new { id = createdSession.Id }, createdSession);
    }

    [HttpPost("trainings")]
    public async Task<ActionResult<TrainingDto>> CreateTraining([FromBody] CreateTrainingRequest request)
    {
        var cmd = _mapper.Map<CreateTrainingRequest, CreateTrainingCommand>(request);
        var createdTraining = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetTraining), new { id = createdTraining.Id }, createdTraining);
    }

    [HttpGet("trainings/{id}")]
    public async Task<ActionResult<TrainingDto>> GetTraining([FromRoute] GetTrainingByIdRequest request)
    {
        var query = _mapper.Map<GetTrainingByIdRequest, GetTrainingByIdQuery>(request);
        var training = await _mediator.Send(query);
        if (training == null)
            return NotFound();
        return Ok(training);
    }

    [HttpPut("sessions/{id}")]
    public async Task<IActionResult> UpdateTrainingSession([FromRoute] int id, [FromBody] UpdateTrainingSessionRequest request)
    {
        if (id != request.Id)
            return BadRequest();

        var cmd = _mapper.Map<UpdateTrainingSessionRequest, UpdateTrainingSessionCommand>(request);
        var updated = await _mediator.Send(cmd);
        if (updated is null)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("sessions/{id}")]
    public async Task<IActionResult> DeleteTrainingSession([FromRoute] DeleteTrainingSessionRequest request)
    {
        var cmd = _mapper.Map<DeleteTrainingSessionRequest, DeleteTrainingSessionCommand>(request);
        await _mediator.Send(cmd);
        return NoContent();
    }
}
