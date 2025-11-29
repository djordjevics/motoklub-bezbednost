using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Services;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrainingsController : ControllerBase
{
    private readonly ITrainingService _trainingService;

    public TrainingsController(ITrainingService trainingService)
    {
        _trainingService = trainingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainingSession>>> GetTrainingSessions()
    {
        var sessions = await _trainingService.GetAllTrainingSessionsAsync();
        return Ok(sessions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrainingSession>> GetTrainingSession(int id)
    {
        var session = await _trainingService.GetTrainingSessionByIdAsync(id);
        if (session == null)
            return NotFound();
        return Ok(session);
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<Training>>> GetTrainingsByMember(int memberId)
    {
        var trainings = await _trainingService.GetTrainingsByMemberIdAsync(memberId);
        return Ok(trainings);
    }

    [HttpPost("sessions")]
    public async Task<ActionResult<TrainingSession>> CreateTrainingSession([FromBody] TrainingSession session)
    {
        var createdSession = await _trainingService.CreateTrainingSessionAsync(session);
        return CreatedAtAction(nameof(GetTrainingSession), new { id = createdSession.Id }, createdSession);
    }

    [HttpPost("trainings")]
    public async Task<ActionResult<Training>> CreateTraining([FromBody] Training training)
    {
        var createdTraining = await _trainingService.CreateTrainingAsync(training);
        return CreatedAtAction(nameof(GetTraining), new { id = createdTraining.Id }, createdTraining);
    }

    [HttpGet("trainings/{id}")]
    public async Task<ActionResult<Training>> GetTraining(int id)
    {
        var training = await _trainingService.GetTrainingByIdAsync(id);
        if (training == null)
            return NotFound();
        return Ok(training);
    }

    [HttpPut("sessions/{id}")]
    public async Task<IActionResult> UpdateTrainingSession(int id, [FromBody] TrainingSession session)
    {
        if (id != session.Id)
            return BadRequest();
        
        await _trainingService.UpdateTrainingSessionAsync(session);
        return NoContent();
    }

    [HttpDelete("sessions/{id}")]
    public async Task<IActionResult> DeleteTrainingSession(int id)
    {
        await _trainingService.DeleteTrainingSessionAsync(id);
        return NoContent();
    }
}

