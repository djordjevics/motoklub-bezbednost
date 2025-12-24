using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<MemberDto>> GetAllMembersAsync()
        => (await _memberRepository.GetAllWithDetailsAsync()).ToDto();

    public async Task<MemberDto?> GetMemberByIdAsync(int id)
    {
        var entity = await _memberRepository.GetByIdWithDetailsAsync(id);
        return entity?.ToDto();
    }

    public async Task<MemberDto> CreateMemberAsync(MemberDto member)
    {
        var entity = member.ToEntity();
        var created = await _memberRepository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task UpdateMemberAsync(MemberDto member)
    {
        var existing = await _memberRepository.GetByIdWithDetailsAsync(member.Id);
        if (existing == null)
        {
            throw new InvalidOperationException($"Member with id {member.Id} not found.");
        }

        member.ToEntity(existing);
        await _memberRepository.UpdateAsync(existing);
    }

    public async Task DeleteMemberAsync(int id)
    {
        await _memberRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<MemberDto>> SearchMembersAsync(string query)
        => (await _memberRepository.SearchAsync(query)).ToDto();
}


