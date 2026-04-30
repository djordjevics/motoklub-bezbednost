using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public interface IMotorcycleRepository : IRepository<MotorcycleDb>
{
    Task<IEnumerable<MotorcycleDb>> GetByMemberIdAsync(int memberId);
}


