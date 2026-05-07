using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoklubBezbednost.API.Models.Requests;
using MotoklubBezbednost.API.Models.Responses;
using MotoklubBezbednost.Business.Cqrs.Comments.Commands;
using MotoklubBezbednost.Business.Cqrs.Comments.Queries;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CommentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<CommentResponse>>> GetCommentsByMember(int memberId)
    {
        var query = _mapper.Map<GetCommentsByMemberQuery>(new GetCommentsByMemberRequest { MemberId = memberId });
        var comments = await _mediator.Send(query);
        return Ok(_mapper.Map<IEnumerable<CommentResponse>>(comments));
    }

    [HttpPost]
    public async Task<ActionResult<CommentResponse>> CreateComment([FromBody] CreateCommentRequest request)
    {
        var command = _mapper.Map<CreateCommentCommand>(request);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCommentsByMember), new { memberId = request.MemberId }, _mapper.Map<CommentResponse>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentRequest request)
    {
        request.Id = id;
        var command = _mapper.Map<UpdateCommentCommand>(request);
        var updated = await _mediator.Send(command);
        if (updated is null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        await _mediator.Send(new DeleteCommentCommand { Id = id });
        return NoContent();
    }
}
