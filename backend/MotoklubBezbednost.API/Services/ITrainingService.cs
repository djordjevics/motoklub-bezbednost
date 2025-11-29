using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Services;

public interface ITrainingService
{
    Task<IEnumerable<TrainingSession>> GetAllTrainingSessionsAsync();
    Task<TrainingSession?> GetTrainingSessionByIdAsync(int id);
    Task<TrainingSession> CreateTrainingSessionAsync(TrainingSession session);
    Task UpdateTrainingSessionAsync(TrainingSession session);
    Task DeleteTrainingSessionAsync(int id);
    Task<IEnumerable<Training>> GetTrainingsByMemberIdAsync(int memberId);
    Task<Training?> GetTrainingByIdAsync(int id);
    Task<Training> CreateTrainingAsync(Training training);
}

