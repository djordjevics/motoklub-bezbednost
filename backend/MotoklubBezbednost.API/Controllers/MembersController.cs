using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Cqrs.Members.Queries;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public MembersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers([FromQuery] GetAllMembersRequest request)
    {
        var query = _mapper.Map<GetAllMembersRequest, GetAllMembersQuery>(request);
        var members = await _mediator.Send(query);
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto>> GetMember([FromRoute] GetMemberByIdRequest request)
    {
        var query = _mapper.Map<GetMemberByIdRequest, GetMemberByIdQuery>(request);
        var member = await _mediator.Send(query);
        if (member == null)
            return NotFound();
        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<MemberDto>> CreateMember([FromBody] CreateMemberRequest request)
    {
        var cmd = _mapper.Map<CreateMemberRequest, CreateMemberCommand>(request);
        var createdMember = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetMember), new { id = createdMember.Id }, createdMember);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMember([FromRoute] int id, [FromBody] UpdateMemberRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        var cmd = _mapper.Map<UpdateMemberRequest, UpdateMemberCommand>(request);
        var updated = await _mediator.Send(cmd);
        if (updated is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMember([FromRoute] DeleteMemberRequest request)
    {
        var cmd = _mapper.Map<DeleteMemberRequest, DeleteMemberCommand>(request);
        await _mediator.Send(cmd);
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<MemberDto>>> SearchMembers([FromQuery] SearchMembersRequest request)
    {
        var query = _mapper.Map<SearchMembersRequest, SearchMembersQuery>(request);
        var members = await _mediator.Send(query);
        return Ok(members);
    }
}

