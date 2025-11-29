using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.API.Data;
using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Repositories;

public class TrainingRepository : Repository<Training>, ITrainingRepository
{
    public TrainingRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Training>> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .Include(t => t.TrainingSession)
                .ThenInclude(ts => ts.Level)
            .Include(t => t.Motorcycle)
            .Where(t => t.MemberId == memberId)
            .OrderByDescending(t => t.TrainingSession.TheoryDate ?? t.TrainingSession.PolygonDate ?? DateTime.MinValue)
            .ToListAsync();
    }

    public async Task<Training?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(t => t.Member)
            .Include(t => t.Motorcycle)
            .Include(t => t.TrainingSession)
                .ThenInclude(ts => ts.Level)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}

