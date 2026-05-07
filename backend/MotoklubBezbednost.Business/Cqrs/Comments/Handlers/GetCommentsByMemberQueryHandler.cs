using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Comments.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Comments.Handlers;

public sealed class GetCommentsByMemberQueryHandler : IRequestHandler<GetCommentsByMemberQuery, IEnumerable<CommentDto>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public GetCommentsByMemberQueryHandler(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByMemberQuery request, CancellationToken cancellationToken)
    {
        var entities = await _commentRepository.FindAsync(c => c.MemberId == request.MemberId);
        // Newest first; CreationTime can be null on legacy rows so coalesce to MinValue.
        return entities
            .OrderByDescending(c => c.CreationTime ?? DateTime.MinValue)
            .Select(e => _mapper.Map<CommentDto>(e));
    }
}
