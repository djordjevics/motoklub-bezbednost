using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoklubBezbednost.API.Models.Requests;
using MotoklubBezbednost.API.Models.Responses;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<ActionResult<IEnumerable<TrainingSessionResponse>>> GetTrainingSessions()
    {
        var sessions = await _mediator.Send(new GetAllTrainingSessionsQuery());
        return Ok(_mapper.Map<IEnumerable<TrainingSessionResponse>>(sessions));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrainingSessionResponse>> GetTrainingSession(int id)
    {
        var query = _mapper.Map<GetTrainingSessionByIdQuery>(new GetTrainingSessionByIdRequest { Id = id });
        var session = await _mediator.Send(query);
        if (session == null)
            return NotFound();
        return Ok(_mapper.Map<TrainingSessionResponse>(session));
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<TrainingResponse>>> GetTrainingsByMember(int memberId)
    {
        var query = _mapper.Map<GetTrainingsByMemberQuery>(new GetTrainingsByMemberRequest { MemberId = memberId });
        var trainings = await _mediator.Send(query);
        return Ok(_mapper.Map<IEnumerable<TrainingResponse>>(trainings));
    }

    [HttpPost("sessions")]
    public async Task<ActionResult<TrainingSessionResponse>> CreateTrainingSession([FromBody] CreateTrainingSessionRequest request)
    {
        var command = _mapper.Map<CreateTrainingSessionCommand>(request);
        var createdSession = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTrainingSession), new { id = createdSession.Id }, _mapper.Map<TrainingSessionResponse>(createdSession));
    }

    [HttpPost("trainings")]
    public async Task<ActionResult<TrainingResponse>> CreateTraining([FromBody] CreateTrainingRequest request)
    {
        var command = _mapper.Map<CreateTrainingCommand>(request);
        var createdTraining = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTraining), new { id = createdTraining.Id }, _mapper.Map<TrainingResponse>(createdTraining));
    }

    [HttpGet("trainings/{id}")]
    public async Task<ActionResult<TrainingResponse>> GetTraining(int id)
    {
        var query = _mapper.Map<GetTrainingByIdQuery>(new GetTrainingByIdRequest { Id = id });
        var training = await _mediator.Send(query);
        if (training == null)
            return NotFound();
        return Ok(_mapper.Map<TrainingResponse>(training));
    }

    [HttpPut("sessions/{id}")]
    public async Task<IActionResult> UpdateTrainingSession(int id, [FromBody] UpdateTrainingSessionRequest request)
    {
        request.Id = id;
        var command = _mapper.Map<UpdateTrainingSessionCommand>(request);
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
