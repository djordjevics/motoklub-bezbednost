using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Repositories;

namespace MotoklubBezbednost.API.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<Member>> GetAllMembersAsync()
    {
        return await _memberRepository.GetAllWithDetailsAsync();
    }

    public async Task<Member?> GetMemberByIdAsync(int id)
    {
        return await _memberRepository.GetByIdWithDetailsAsync(id);
    }

    public async Task<Member> CreateMemberAsync(Member member)
    {
        return await _memberRepository.AddAsync(member);
    }

    public async Task UpdateMemberAsync(Member member)
    {
        await _memberRepository.UpdateAsync(member);
    }

    public async Task DeleteMemberAsync(int id)
    {
        await _memberRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Member>> SearchMembersAsync(string query)
    {
        return await _memberRepository.SearchAsync(query);
    }
}

