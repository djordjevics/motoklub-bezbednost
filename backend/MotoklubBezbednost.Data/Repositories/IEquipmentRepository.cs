using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public interface IEquipmentRepository : IRepository<EquipmentDb>
{
    Task<EquipmentDb?> GetByMemberIdAsync(int memberId);
}


