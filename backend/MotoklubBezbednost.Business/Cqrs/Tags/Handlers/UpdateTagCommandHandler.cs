using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Tags.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Tags.Handlers;

public sealed class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, TagDto?>
{
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTagCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TagDto?> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var existing = await _tagRepository.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(request, existing);
        await _tagRepository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TagDto>(existing);
    }
}
