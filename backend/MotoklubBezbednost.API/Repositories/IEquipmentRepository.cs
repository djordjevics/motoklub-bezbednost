using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Repositories;

public interface IEquipmentRepository : IRepository<Equipment>
{
    Task<Equipment?> GetByMemberIdAsync(int memberId);
    Task<IEnumerable<Equipment>> GetAllWithMemberAsync();
}

