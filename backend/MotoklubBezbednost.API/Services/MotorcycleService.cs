using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.API.Data;
using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Services;

public class MotorcycleService : IMotorcycleService
{
    private readonly ApplicationDbContext _context;

    public MotorcycleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Motorcycle>> GetAllMotorcyclesAsync()
    {
        return await _context.Motorcycles
            .Include(m => m.Member)
            .ToListAsync();
    }

    public async Task<Motorcycle?> GetMotorcycleByIdAsync(int id)
    {
        return await _context.Motorcycles
            .Include(m => m.Member)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByMemberIdAsync(int memberId)
    {
        return await _context.Motorcycles
            .Where(m => m.MemberId == memberId)
            .ToListAsync();
    }

    public async Task<Motorcycle> CreateMotorcycleAsync(Motorcycle motorcycle)
    {
        _context.Motorcycles.Add(motorcycle);
        await _context.SaveChangesAsync();
        return motorcycle;
    }

    public async Task UpdateMotorcycleAsync(Motorcycle motorcycle)
    {
        _context.Motorcycles.Update(motorcycle);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMotorcycleAsync(int id)
    {
        var motorcycle = await _context.Motorcycles.FindAsync(id);
        if (motorcycle != null)
        {
            _context.Motorcycles.Remove(motorcycle);
            await _context.SaveChangesAsync();
        }
    }
}

