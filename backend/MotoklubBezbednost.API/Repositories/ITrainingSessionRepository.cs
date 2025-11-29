using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Repositories;

public interface ITrainingSessionRepository : IRepository<TrainingSession>
{
    Task<IEnumerable<TrainingSession>> GetAllWithDetailsAsync();
    Task<TrainingSession?> GetByIdWithDetailsAsync(int id);
}

