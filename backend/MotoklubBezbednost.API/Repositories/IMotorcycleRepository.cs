using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Repositories;

public interface IMotorcycleRepository : IRepository<Motorcycle>
{
    Task<IEnumerable<Motorcycle>> GetByMemberIdAsync(int memberId);
    Task<IEnumerable<Motorcycle>> GetAllWithMemberAsync();
}

