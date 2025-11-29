using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.API.Data;
using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Repositories;

public class MotorcycleRepository : Repository<Motorcycle>, IMotorcycleRepository
{
    public MotorcycleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Motorcycle>> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .Where(m => m.MemberId == memberId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Motorcycle>> GetAllWithMemberAsync()
    {
        return await _dbSet
            .Include(m => m.Member)
            .ToListAsync();
    }

    public override async Task<Motorcycle?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Member)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}

