using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Services;

public interface ITrainingService
{
    Task<IEnumerable<TrainingSessionDto>> GetAllTrainingSessionsAsync();
    Task<TrainingSessionDto?> GetTrainingSessionByIdAsync(int id);
    Task<TrainingSessionDto> CreateTrainingSessionAsync(TrainingSessionDto session);
    Task UpdateTrainingSessionAsync(TrainingSessionDto session);
    Task DeleteTrainingSessionAsync(int id);
    Task<IEnumerable<TrainingDto>> GetTrainingsByMemberIdAsync(int memberId);
    Task<TrainingDto?> GetTrainingByIdAsync(int id);
    Task<TrainingDto> CreateTrainingAsync(TrainingDto training);
}


