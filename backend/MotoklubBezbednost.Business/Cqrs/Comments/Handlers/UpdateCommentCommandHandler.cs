using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Comments.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Comments.Handlers;

public sealed class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentDto?>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommentDto?> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var existing = await _commentRepository.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(request, existing);
        existing.EditTime = DateTime.UtcNow;
        await _commentRepository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CommentDto>(existing);
    }
}
