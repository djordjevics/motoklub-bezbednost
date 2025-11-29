using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.API.Data;
using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Services;

public class MemberService : IMemberService
{
    private readonly ApplicationDbContext _context;

    public MemberService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Member>> GetAllMembersAsync()
    {
        return await _context.Members
            .Include(m => m.Motorcycles)
            .Include(m => m.Equipment)
            .Include(m => m.Trainings)
            .Include(m => m.MemberType)
            .ToListAsync();
    }

    public async Task<Member?> GetMemberByIdAsync(int id)
    {
        return await _context.Members
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

    public async Task<Member> CreateMemberAsync(Member member)
    {
        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task UpdateMemberAsync(Member member)
    {
        _context.Members.Update(member);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMemberAsync(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member != null)
        {
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Member>> SearchMembersAsync(string query)
    {
        var searchTerm = query.ToLower();
        return await _context.Members
            .Where(m => m.Name.ToLower().Contains(searchTerm) ||
                       m.Surname.ToLower().Contains(searchTerm) ||
                       (m.Email != null && m.Email.ToLower().Contains(searchTerm)))
            .ToListAsync();
    }
}

