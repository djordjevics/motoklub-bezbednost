using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;
using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.API.Mappers;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TrainingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainingSessionDto>>> GetTrainingSessions()
    {
        var sessions = await _mediator.Send(new GetAllTrainingSessionsQuery());
        return Ok(sessions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrainingSessionDto>> GetTrainingSession(int id)
    {
        var request = new GetTrainingSessionByIdRequest { Id = id };
        var query = request.ToQuery();
        var session = await _mediator.Send(query);
        if (session == null)
            return NotFound();
        return Ok(session);
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<TrainingDto>>> GetTrainingsByMember(int memberId)
    {
        var request = new GetTrainingsByMemberRequest { MemberId = memberId };
        var query = request.ToQuery();
        var trainings = await _mediator.Send(query);
        return Ok(trainings);
    }

    [HttpPost("sessions")]
    public async Task<ActionResult<TrainingSessionDto>> CreateTrainingSession([FromBody] CreateTrainingSessionRequest request)
    {
        var command = request.ToCommand();
        var createdSession = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTrainingSession), new { id = createdSession.Id }, createdSession);
    }

    [HttpPost("trainings")]
    public async Task<ActionResult<TrainingDto>> CreateTraining([FromBody] CreateTrainingRequest request)
    {
        var command = request.ToCommand();
        var createdTraining = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTraining), new { id = createdTraining.Id }, createdTraining);
    }

    [HttpGet("trainings/{id}")]
    public async Task<ActionResult<TrainingDto>> GetTraining(int id)
    {
        var request = new GetTrainingByIdRequest { Id = id };
        var query = request.ToQuery();
        var training = await _mediator.Send(query);
        if (training == null)
            return NotFound();
        return Ok(training);
    }

    [HttpPut("sessions/{id}")]
    public async Task<IActionResult> UpdateTrainingSession(int id, [FromBody] UpdateTrainingSessionRequest request)
    {
        request.Id = id;
        var command = request.ToCommand();
        var updated = await _mediator.Send(command);
        if (updated is null)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("sessions/{id}")]
    public async Task<IActionResult> DeleteTrainingSession(int id)
    {
        await _mediator.Send(new DeleteTrainingSessionCommand { Id = id });
        return NoContent();
    }
}
