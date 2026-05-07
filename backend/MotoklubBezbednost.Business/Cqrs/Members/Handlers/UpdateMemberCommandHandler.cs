using AutoMapper;
using MediatR;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Rules;
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

        var wasInactiveRecord = existing.MembershipExemptManual;

        // Inactive (archival) rows: allow only flipping MembershipExemptManual back to active.
        if (wasInactiveRecord)
        {
            if (request.MembershipExemptManual.HasValue)
            {
                existing.MembershipExemptManual = request.MembershipExemptManual.Value;
            }

            existing.LastModificationTimestamp = DateTime.UtcNow;
        }
        else
        {
            _mapper.Map(request, existing);
        }

        await _memberRepository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<MemberDto>(existing);
        MembershipRules.EnrichMembershipExempt(existing, dto, DateTime.Today);
        return dto;
    }
}


