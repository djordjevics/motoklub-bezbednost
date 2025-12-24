using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Services;

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

    public async Task<IEnumerable<TrainingSessionDto>> GetAllTrainingSessionsAsync()
        => (await _trainingSessionRepository.GetAllWithDetailsAsync()).ToDto();

    public async Task<TrainingSessionDto?> GetTrainingSessionByIdAsync(int id)
    {
        var entity = await _trainingSessionRepository.GetByIdWithDetailsAsync(id);
        return entity?.ToDto();
    }

    public async Task<TrainingSessionDto> CreateTrainingSessionAsync(TrainingSessionDto session)
    {
        var entity = new MotoklubBezbednost.Data.Models.TrainingSessionDb
        {
            TheoryDate = session.TheoryDate,
            PolygonDate = session.PolygonDate,
            City = session.City,
            Price = session.Price,
            Instructors = session.Instructors,
            Note = session.Note
        };

        var created = await _trainingSessionRepository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task UpdateTrainingSessionAsync(TrainingSessionDto session)
    {
        var existing = await _trainingSessionRepository.GetByIdWithDetailsAsync(session.Id);
        if (existing == null)
        {
            throw new InvalidOperationException($"TrainingSession with id {session.Id} not found.");
        }

        existing.TheoryDate = session.TheoryDate;
        existing.PolygonDate = session.PolygonDate;
        existing.City = session.City;
        existing.Price = session.Price;
        existing.Instructors = session.Instructors;
        existing.Note = session.Note;

        await _trainingSessionRepository.UpdateAsync(existing);
    }

    public async Task DeleteTrainingSessionAsync(int id)
    {
        await _trainingSessionRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<TrainingDto>> GetTrainingsByMemberIdAsync(int memberId)
        => (await _trainingRepository.GetByMemberIdAsync(memberId)).ToDto();

    public async Task<TrainingDto?> GetTrainingByIdAsync(int id)
    {
        var entity = await _trainingRepository.GetByIdWithDetailsAsync(id);
        return entity?.ToDto();
    }

    public async Task<TrainingDto> CreateTrainingAsync(TrainingDto training)
    {
        var entity = new MotoklubBezbednost.Data.Models.TrainingDb
        {
            IsCertificateIssued = training.IsCertificateIssued,
            Note = training.Note
            // Relationships (Member, Motorcycle, TrainingSession) should be set via FKs or handled elsewhere.
        };

        var created = await _trainingRepository.AddAsync(entity);
        return created.ToDto();
    }
}


