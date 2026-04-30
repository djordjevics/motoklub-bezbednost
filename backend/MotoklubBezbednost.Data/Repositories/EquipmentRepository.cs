using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public class EquipmentRepository : Repository<EquipmentDb>, IEquipmentRepository
{
    public EquipmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<EquipmentDb?> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => EF.Property<int>(e, "MemberId") == memberId);
    }

    public override async Task<EquipmentDb?> GetByIdAsync(int id)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}


