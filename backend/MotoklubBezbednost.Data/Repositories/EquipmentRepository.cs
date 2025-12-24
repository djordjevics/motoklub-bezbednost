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
            .Include(e => e.Member)
            .FirstOrDefaultAsync(e => EF.Property<int>(e, "MemberId") == memberId);
    }

    public async Task<IEnumerable<EquipmentDb>> GetAllWithMemberAsync()
    {
        return await _dbSet
            .Include(e => e.Member)
            .ToListAsync();
    }

    public override async Task<EquipmentDb?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(e => e.Member)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}


