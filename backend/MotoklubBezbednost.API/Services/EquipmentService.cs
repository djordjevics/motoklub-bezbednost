using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.API.Data;
using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Services;

public class EquipmentService : IEquipmentService
{
    private readonly ApplicationDbContext _context;

    public EquipmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Equipment>> GetAllEquipmentAsync()
    {
        return await _context.Equipment
            .Include(e => e.Member)
            .ToListAsync();
    }

    public async Task<Equipment?> GetEquipmentByIdAsync(int id)
    {
        return await _context.Equipment
            .Include(e => e.Member)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Equipment>> GetEquipmentByMemberIdAsync(int memberId)
    {
        return await _context.Equipment
            .Where(e => e.MemberId == memberId)
            .ToListAsync();
    }

    public async Task<Equipment> CreateEquipmentAsync(Equipment equipment)
    {
        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();
        return equipment;
    }

    public async Task UpdateEquipmentAsync(Equipment equipment)
    {
        _context.Equipment.Update(equipment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEquipmentAsync(int id)
    {
        var equipment = await _context.Equipment.FindAsync(id);
        if (equipment != null)
        {
            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();
        }
    }
}

