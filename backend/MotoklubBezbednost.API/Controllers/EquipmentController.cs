using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Services;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;

    public EquipmentController(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Equipment>>> GetEquipment()
    {
        var equipment = await _equipmentService.GetAllEquipmentAsync();
        return Ok(equipment);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Equipment>> GetEquipment(int id)
    {
        var equipment = await _equipmentService.GetEquipmentByIdAsync(id);
        if (equipment == null)
            return NotFound();
        return Ok(equipment);
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<Equipment>>> GetEquipmentByMember(int memberId)
    {
        var equipment = await _equipmentService.GetEquipmentByMemberIdAsync(memberId);
        return Ok(equipment);
    }

    [HttpPost]
    public async Task<ActionResult<Equipment>> CreateEquipment([FromBody] Equipment equipment)
    {
        var createdEquipment = await _equipmentService.CreateEquipmentAsync(equipment);
        return CreatedAtAction(nameof(GetEquipment), new { id = createdEquipment.Id }, createdEquipment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEquipment(int id, [FromBody] Equipment equipment)
    {
        if (id != equipment.Id)
            return BadRequest();
        
        await _equipmentService.UpdateEquipmentAsync(equipment);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipment(int id)
    {
        await _equipmentService.DeleteEquipmentAsync(id);
        return NoContent();
    }
}

