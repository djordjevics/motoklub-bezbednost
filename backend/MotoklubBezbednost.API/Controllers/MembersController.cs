using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoklubBezbednost.API.Models.Requests;
using MotoklubBezbednost.API.Models.Responses;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Cqrs.Members.Queries;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetMembers()
    {
        var members = await _mediator.Send(new GetAllMembersQuery());
        return Ok(_mapper.Map<IEnumerable<MemberResponse>>(members));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberResponse>> GetMember(int id)
    {
        var query = _mapper.Map<GetMemberByIdQuery>(new GetMemberByIdRequest { Id = id });
        var member = await _mediator.Send(query);
        if (member == null)
            return NotFound();
        return Ok(_mapper.Map<MemberResponse>(member));
    }

    [HttpPost]
    public async Task<ActionResult<MemberResponse>> CreateMember([FromBody] CreateMemberRequest request)
    {
        var command = _mapper.Map<CreateMemberCommand>(request);
        var createdMember = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetMember), new { id = createdMember.Id }, _mapper.Map<MemberResponse>(createdMember));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMember(int id, [FromBody] UpdateMemberRequest request)
    {
        request.Id = id;
        var command = _mapper.Map<UpdateMemberCommand>(request);
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
    public async Task<ActionResult<IEnumerable<MemberResponse>>> SearchMembers([FromQuery] string? query)
    {
        var searchQuery = _mapper.Map<SearchMembersQuery>(new SearchMembersRequest { Query = query });
        var members = await _mediator.Send(searchQuery);
        return Ok(_mapper.Map<IEnumerable<MemberResponse>>(members));
    }
}
