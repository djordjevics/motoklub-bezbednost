using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Repositories;

public interface IMemberRepository : IRepository<Member>
{
    Task<Member?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Member>> GetAllWithDetailsAsync();
    Task<IEnumerable<Member>> SearchAsync(string query);
}

