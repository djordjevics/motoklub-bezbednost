using MediatR;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Cqrs.Members.Handlers;

public sealed class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Unit>
{
    private readonly IMemberRepository _memberRepository;

    public DeleteMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Unit> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        await _memberRepository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}


