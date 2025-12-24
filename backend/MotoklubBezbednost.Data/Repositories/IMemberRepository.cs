using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public interface IMemberRepository : IRepository<MemberDb>
{
    Task<MemberDb?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<MemberDb>> GetAllWithDetailsAsync();
    Task<IEnumerable<MemberDb>> SearchAsync(string query);
}


