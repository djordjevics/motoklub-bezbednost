using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Members.Handlers;

public sealed class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, MemberDto?>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MemberDto?> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var existing = await _memberRepository.GetByIdWithDetailsAsync(request.Id);
        if (existing == null)
        {
            return null;
        }

        _mapper.Map(request, existing);
        await _memberRepository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<MemberDto>(existing);
    }
}


