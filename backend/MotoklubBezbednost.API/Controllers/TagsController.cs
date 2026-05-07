using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoklubBezbednost.API.Models.Requests;
using MotoklubBezbednost.API.Models.Responses;
using MotoklubBezbednost.Business.Cqrs.Tags.Commands;
using MotoklubBezbednost.Business.Cqrs.Tags.Queries;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TagsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<TagResponse>>> GetTagsByMember(int memberId)
    {
        var query = _mapper.Map<GetTagsByMemberQuery>(new GetTagsByMemberRequest { MemberId = memberId });
        var tags = await _mediator.Send(query);
        return Ok(_mapper.Map<IEnumerable<TagResponse>>(tags));
    }

    [HttpPost]
    public async Task<ActionResult<TagResponse>> CreateTag([FromBody] CreateTagRequest request)
    {
        var command = _mapper.Map<CreateTagCommand>(request);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTagsByMember), new { memberId = request.MemberId }, _mapper.Map<TagResponse>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] UpdateTagRequest request)
    {
        request.Id = id;
        var command = _mapper.Map<UpdateTagCommand>(request);
        var updated = await _mediator.Send(command);
        if (updated is null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        await _mediator.Send(new DeleteTagCommand { Id = id });
        return NoContent();
    }
}
