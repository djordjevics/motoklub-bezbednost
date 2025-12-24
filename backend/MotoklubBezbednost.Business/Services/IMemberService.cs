using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Services;

public interface IMemberService
{
    Task<IEnumerable<MemberDto>> GetAllMembersAsync();
    Task<MemberDto?> GetMemberByIdAsync(int id);
    Task<MemberDto> CreateMemberAsync(MemberDto member);
    Task UpdateMemberAsync(MemberDto member);
    Task DeleteMemberAsync(int id);
    Task<IEnumerable<MemberDto>> SearchMembersAsync(string query);
}


