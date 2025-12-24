using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Business.Services;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MotorcyclesController : ControllerBase
{
    private readonly IMotorcycleService _motorcycleService;

    public MotorcyclesController(IMotorcycleService motorcycleService)
    {
        _motorcycleService = motorcycleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Motorcycle>>> GetMotorcycles()
    {
        var motorcycles = await _motorcycleService.GetAllMotorcyclesAsync();
        return Ok(motorcycles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Motorcycle>> GetMotorcycle(int id)
    {
        var motorcycle = await _motorcycleService.GetMotorcycleByIdAsync(id);
        if (motorcycle == null)
            return NotFound();
        return Ok(motorcycle);
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<Motorcycle>>> GetMotorcyclesByMember(int memberId)
    {
        var motorcycles = await _motorcycleService.GetMotorcyclesByMemberIdAsync(memberId);
        return Ok(motorcycles);
    }

    [HttpPost]
    public async Task<ActionResult<Motorcycle>> CreateMotorcycle([FromBody] Motorcycle motorcycle)
    {
        var createdMotorcycle = await _motorcycleService.CreateMotorcycleAsync(motorcycle);
        return CreatedAtAction(nameof(GetMotorcycle), new { id = createdMotorcycle.Id }, createdMotorcycle);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMotorcycle(int id, [FromBody] Motorcycle motorcycle)
    {
        if (id != motorcycle.Id)
            return BadRequest();
        
        await _motorcycleService.UpdateMotorcycleAsync(motorcycle);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMotorcycle(int id)
    {
        await _motorcycleService.DeleteMotorcycleAsync(id);
        return NoContent();
    }
}

