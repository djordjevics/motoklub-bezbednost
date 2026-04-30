using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public class TrainingSessionRepository : Repository<TrainingSessionDb>, ITrainingSessionRepository
{
    public TrainingSessionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TrainingSessionDb>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(ts => ts.Trainings)
            .Include(ts => ts.Level)
            .OrderByDescending(ts => ts.TheoryDate ?? ts.PolygonDate ?? DateTime.MinValue)
            .ToListAsync();
    }

    public async Task<TrainingSessionDb?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(ts => ts.Trainings)
            .Include(ts => ts.Trainings)
                .ThenInclude(t => t.Motorcycle)
            .Include(ts => ts.Level)
            .FirstOrDefaultAsync(ts => ts.Id == id);
    }
}


