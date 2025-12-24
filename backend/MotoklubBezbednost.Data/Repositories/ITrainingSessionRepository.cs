using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public interface ITrainingSessionRepository : IRepository<TrainingSessionDb>
{
    Task<IEnumerable<TrainingSessionDb>> GetAllWithDetailsAsync();
    Task<TrainingSessionDb?> GetByIdWithDetailsAsync(int id);
}


