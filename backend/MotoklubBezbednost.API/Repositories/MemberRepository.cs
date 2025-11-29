using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.API.Data;
using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    public MemberRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Member?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Motorcycles)
            .Include(m => m.Equipment)
            .Include(m => m.Trainings)
                .ThenInclude(t => t.TrainingSession)
            .Include(m => m.MemberType)
            .Include(m => m.MembershipPayments)
            .Include(m => m.Comments)
            .Include(m => m.Tags)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Member>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(m => m.Motorcycles)
            .Include(m => m.Equipment)
            .Include(m => m.Trainings)
            .Include(m => m.MemberType)
            .ToListAsync();
    }

    public async Task<IEnumerable<Member>> SearchAsync(string query)
    {
        var searchTerm = query.ToLower();
        return await _dbSet
            .Where(m => m.Name.ToLower().Contains(searchTerm) ||
                       m.Surname.ToLower().Contains(searchTerm) ||
                       (m.Email != null && m.Email.ToLower().Contains(searchTerm)))
            .ToListAsync();
    }
}

