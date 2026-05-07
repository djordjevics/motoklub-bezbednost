using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public interface ITrainingRepository : IRepository<TrainingDb>
{
    Task<IEnumerable<TrainingDb>> GetByMemberIdAsync(int memberId);
    Task<IEnumerable<TrainingDb>> GetBySessionIdAsync(int trainingSessionId);
    Task<TrainingDb?> GetByIdWithDetailsAsync(int id);
}


