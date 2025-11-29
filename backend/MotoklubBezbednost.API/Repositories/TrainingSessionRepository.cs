using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.API.Data;
using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Repositories;

public class TrainingSessionRepository : Repository<TrainingSession>, ITrainingSessionRepository
{
    public TrainingSessionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TrainingSession>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(ts => ts.Trainings)
                .ThenInclude(t => t.Member)
            .Include(ts => ts.Level)
            .OrderByDescending(ts => ts.TheoryDate ?? ts.PolygonDate ?? DateTime.MinValue)
            .ToListAsync();
    }

    public async Task<TrainingSession?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(ts => ts.Trainings)
                .ThenInclude(t => t.Member)
            .Include(ts => ts.Trainings)
                .ThenInclude(t => t.Motorcycle)
            .Include(ts => ts.Level)
            .FirstOrDefaultAsync(ts => ts.Id == id);
    }
}

