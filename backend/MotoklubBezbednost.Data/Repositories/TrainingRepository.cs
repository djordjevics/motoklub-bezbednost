using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public class TrainingRepository : Repository<TrainingDb>, ITrainingRepository
{
    public TrainingRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TrainingDb>> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .Include(t => t.TrainingSession)
                .ThenInclude(ts => ts.Level)
            .Include(t => t.Motorcycle)
            .Where(t => EF.Property<int>(t, "MemberId") == memberId)
            .OrderByDescending(t => t.TrainingSession.TheoryDate ?? t.TrainingSession.PolygonDate ?? DateTime.MinValue)
            .ToListAsync();
    }

    public async Task<TrainingDb?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(t => t.Member)
            .Include(t => t.Motorcycle)
            .Include(t => t.TrainingSession)
                .ThenInclude(ts => ts.Level)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}


