using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Repositories;

public interface ITrainingRepository : IRepository<Training>
{
    Task<IEnumerable<Training>> GetByMemberIdAsync(int memberId);
    Task<Training?> GetByIdWithDetailsAsync(int id);
}

