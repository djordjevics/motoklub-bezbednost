using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Cqrs.Members.Queries;
using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.API.Mappers;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers()
    {
        var members = await _mediator.Send(new GetAllMembersQuery());
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto>> GetMember(int id)
    {
        var request = new GetMemberByIdRequest { Id = id };
        var query = request.ToQuery();
        var member = await _mediator.Send(query);
        if (member == null)
            return NotFound();
        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<MemberDto>> CreateMember([FromBody] CreateMemberRequest request)
    {
        var command = request.ToCommand();
        var createdMember = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetMember), new { id = createdMember.Id }, createdMember);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMember(int id, [FromBody] UpdateMemberRequest request)
    {
        request.Id = id;
        var command = request.ToCommand();
        var updated = await _mediator.Send(command);
        if (updated is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        await _mediator.Send(new DeleteMemberCommand { Id = id });
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<MemberDto>>> SearchMembers([FromQuery] string query)
    {
        var request = new SearchMembersRequest { Query = query };
        var searchQuery = request.ToQuery();
        var members = await _mediator.Send(searchQuery);
        return Ok(members);
    }
}

