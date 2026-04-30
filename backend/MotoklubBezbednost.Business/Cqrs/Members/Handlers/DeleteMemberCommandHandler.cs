using MediatR;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;

namespace MotoklubBezbednost.Business.Cqrs.Members.Handlers;

public sealed class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Unit>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        await _memberRepository.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}


