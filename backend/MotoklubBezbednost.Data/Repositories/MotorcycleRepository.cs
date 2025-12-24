using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public class MotorcycleRepository : Repository<MotorcycleDb>, IMotorcycleRepository
{
    public MotorcycleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MotorcycleDb>> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .Include(m => m.Member)
            .Where(m => EF.Property<int>(m, "MemberId") == memberId)
            .ToListAsync();
    }

    public async Task<IEnumerable<MotorcycleDb>> GetAllWithMemberAsync()
    {
        return await _dbSet
            .Include(m => m.Member)
            .ToListAsync();
    }

    public override async Task<MotorcycleDb?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Member)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}


