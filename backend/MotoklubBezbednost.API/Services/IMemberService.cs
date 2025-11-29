using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Services;

public interface IMemberService
{
    Task<IEnumerable<Member>> GetAllMembersAsync();
    Task<Member?> GetMemberByIdAsync(int id);
    Task<Member> CreateMemberAsync(Member member);
    Task UpdateMemberAsync(Member member);
    Task DeleteMemberAsync(int id);
    Task<IEnumerable<Member>> SearchMembersAsync(string query);
}

