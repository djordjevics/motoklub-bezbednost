using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.Business.Cqrs.MembershipPayments.Queries;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Cqrs.MembershipPayments.Handlers;

public sealed class GetMembershipPaymentsByMemberQueryHandler : IRequestHandler<GetMembershipPaymentsByMemberQuery, IEnumerable<MembershipPaymentDto>>
{
    // Direct DbContext access here (instead of the generic repository) so we can Include the PaymentType
    // navigation in a single round trip — the response DTO carries it via AutoMapper.
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMembershipPaymentsByMemberQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MembershipPaymentDto>> Handle(GetMembershipPaymentsByMemberQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Set<MembershipPaymentDb>()
            .AsNoTracking()
            .Include(p => p.PaymentType)
            .Where(p => p.MemberId == request.MemberId)
            .OrderByDescending(p => p.PaymentDate ?? DateTime.MinValue)
            .ToListAsync(cancellationToken);

        return entities.Select(e => _mapper.Map<MembershipPaymentDto>(e));
    }
}
