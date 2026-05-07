using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Tags.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Tags.Handlers;

public sealed class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, TagDto>
{
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTagCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TagDto> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<TagDb>(request);
        var created = await _tagRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TagDto>(created);
    }
}
