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
            .Where(m => EF.Property<int>(m, "MemberId") == memberId)
            .ToListAsync();
    }

    public override async Task<MotorcycleDb?> GetByIdAsync(int id)
    {
        return await _dbSet
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}


