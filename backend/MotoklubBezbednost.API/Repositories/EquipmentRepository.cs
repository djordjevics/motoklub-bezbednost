using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.API.Data;
using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Repositories;

public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
{
    public EquipmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Equipment?> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .Include(e => e.Member)
            .FirstOrDefaultAsync(e => EF.Property<int>(e, "MemberId") == memberId);
    }

    public async Task<IEnumerable<Equipment>> GetAllWithMemberAsync()
    {
        return await _dbSet
            .Include(e => e.Member)
            .ToListAsync();
    }

    public override async Task<Equipment?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(e => e.Member)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}

