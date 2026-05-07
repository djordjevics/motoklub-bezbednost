using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Comments.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Comments.Handlers;

public sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentDto>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<CommentDb>(request);
        // CreationTime is the user-facing comment timestamp; CreationTimestamp is the audit field stamped by the interceptor.
        entity.CreationTime = DateTime.UtcNow;
        var created = await _commentRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CommentDto>(created);
    }
}
