using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.API.Data;
using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Services;

public class TrainingService : ITrainingService
{
    private readonly ApplicationDbContext _context;

    public TrainingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TrainingSession>> GetAllTrainingSessionsAsync()
    {
        return await _context.TrainingSessions
            .Include(ts => ts.Trainings)
                .ThenInclude(t => t.Member)
            .Include(ts => ts.Level)
            .OrderByDescending(ts => ts.TheoryDate ?? ts.PolygonDate ?? DateTime.MinValue)
            .ToListAsync();
    }

    public async Task<TrainingSession?> GetTrainingSessionByIdAsync(int id)
    {
        return await _context.TrainingSessions
            .Include(ts => ts.Trainings)
                .ThenInclude(t => t.Member)
            .Include(ts => ts.Trainings)
                .ThenInclude(t => t.Motorcycle)
            .Include(ts => ts.Level)
            .FirstOrDefaultAsync(ts => ts.Id == id);
    }

    public async Task<TrainingSession> CreateTrainingSessionAsync(TrainingSession session)
    {
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task UpdateTrainingSessionAsync(TrainingSession session)
    {
        _context.TrainingSessions.Update(session);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTrainingSessionAsync(int id)
    {
        var session = await _context.TrainingSessions.FindAsync(id);
        if (session != null)
        {
            _context.TrainingSessions.Remove(session);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Training>> GetTrainingsByMemberIdAsync(int memberId)
    {
        return await _context.Trainings
            .Include(t => t.TrainingSession)
                .ThenInclude(ts => ts.Level)
            .Include(t => t.Motorcycle)
            .Where(t => t.MemberId == memberId)
            .OrderByDescending(t => t.TrainingSession.TheoryDate ?? t.TrainingSession.PolygonDate ?? DateTime.MinValue)
            .ToListAsync();
    }

    public async Task<Training?> GetTrainingByIdAsync(int id)
    {
        return await _context.Trainings
            .Include(t => t.Member)
            .Include(t => t.Motorcycle)
            .Include(t => t.TrainingSession)
                .ThenInclude(ts => ts.Level)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Training> CreateTrainingAsync(Training training)
    {
        _context.Trainings.Add(training);
        await _context.SaveChangesAsync();
        return training;
    }
}

