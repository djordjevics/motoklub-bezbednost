using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Repositories;

namespace MotoklubBezbednost.API.Services;

public class TrainingService : ITrainingService
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly ITrainingRepository _trainingRepository;

    public TrainingService(
        ITrainingSessionRepository trainingSessionRepository,
        ITrainingRepository trainingRepository)
    {
        _trainingSessionRepository = trainingSessionRepository;
        _trainingRepository = trainingRepository;
    }

    public async Task<IEnumerable<TrainingSession>> GetAllTrainingSessionsAsync()
    {
        return await _trainingSessionRepository.GetAllWithDetailsAsync();
    }

    public async Task<TrainingSession?> GetTrainingSessionByIdAsync(int id)
    {
        return await _trainingSessionRepository.GetByIdWithDetailsAsync(id);
    }

    public async Task<TrainingSession> CreateTrainingSessionAsync(TrainingSession session)
    {
        return await _trainingSessionRepository.AddAsync(session);
    }

    public async Task UpdateTrainingSessionAsync(TrainingSession session)
    {
        await _trainingSessionRepository.UpdateAsync(session);
    }

    public async Task DeleteTrainingSessionAsync(int id)
    {
        await _trainingSessionRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Training>> GetTrainingsByMemberIdAsync(int memberId)
    {
        return await _trainingRepository.GetByMemberIdAsync(memberId);
    }

    public async Task<Training?> GetTrainingByIdAsync(int id)
    {
        return await _trainingRepository.GetByIdWithDetailsAsync(id);
    }

    public async Task<Training> CreateTrainingAsync(Training training)
    {
        return await _trainingRepository.AddAsync(training);
    }
}

