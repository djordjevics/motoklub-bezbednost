using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public partial class MemberRepository : Repository<MemberDb>, IMemberRepository
{
    public MemberRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<MemberDb?> GetByIdWithDetailsAsync(int id)
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

    public async Task<IEnumerable<MemberDb>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(m => m.Motorcycles)
            .Include(m => m.Equipment)
            .Include(m => m.Trainings)
            .Include(m => m.MemberType)
            .Include(m => m.Tags)
            .ToListAsync();
    }

    public async Task<IEnumerable<MemberDb>> SearchAsync(string query)
    {
        var trimmed = (query ?? string.Empty).Trim();
        var searchTermLower = trimmed.ToLowerInvariant();
        var today = DateTime.Today;
        var hyphenSearch = TryParseActiveTagPattern(trimmed, out var memberTypeIdGuess, out var memberTypeNameLowerOut, out var tagNumberGuess);

        return await _dbSet
            .AsNoTracking()
            .Include(m => m.MemberType)
            .Include(m => m.Trainings)
            .Include(m => m.Tags)
            .Where(m => m.Name.ToLower().Contains(searchTermLower) ||
                        m.Surname.ToLower().Contains(searchTermLower) ||
                        (m.Email != null && m.Email.ToLower().Contains(searchTermLower)) ||
                        (hyphenSearch &&
                         (
                             (memberTypeIdGuess.HasValue &&
                              m.MemberTypeId == memberTypeIdGuess.Value) ||
                             (!memberTypeIdGuess.HasValue &&
                              m.MemberType != null &&
                              m.MemberType.TypeName != null &&
                              m.MemberType.TypeName.ToLower().Contains(memberTypeNameLowerOut!))
                         ) &&
                         m.Tags.Any(t =>
                             t.TagNumber == tagNumberGuess &&
                             (t.ValidFrom.HasValue || t.ValidTo.HasValue) &&
                             (!t.ValidFrom.HasValue ||
                              t.ValidFrom.Value.Date <= today) &&
                             (!t.ValidTo.HasValue || t.ValidTo.Value.Date >= today))))
            .ToListAsync();
    }

    /// <summary>
    /// Matches SPA <c>formatActiveMemberTagLabel</c>: numeric member type ("group") and numeric tag number,
    /// optionally separated by <c>-</c> with surrounding spaces (e.g. <c>2-42</c> or <c>2 - 42</c>).
    /// Also allows the group segment to partially match a member-type name (e.g. <c>aktiv-10</c> or <c>Akt-10</c>).
    /// </summary>
    private static bool TryParseActiveTagPattern(
        string trimmedQuery,
        out int? memberTypeId,
        out string? memberTypeNameLower,
        out int tagNumber)
    {
        memberTypeId = null;
        memberTypeNameLower = null;
        tagNumber = 0;

        var m = ActiveTagSearchRegex().Match(trimmedQuery);
        if (!m.Success)
        {
            return false;
        }

        var rawGroup = m.Groups["group"].Value.Trim();
        var rawTag = m.Groups["tag"].Value;
        if (string.IsNullOrEmpty(rawGroup))
        {
            return false;
        }

        if (!int.TryParse(rawTag, NumberStyles.Integer, CultureInfo.InvariantCulture, out tagNumber))
        {
            return false;
        }

        if (int.TryParse(rawGroup, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
        {
            memberTypeId = id;
        }
        else
        {
            memberTypeNameLower = rawGroup.ToLowerInvariant();
        }

        return true;
    }

    [GeneratedRegex(@"^\s*(?<group>.+)\s*-\s*(?<tag>\d+)\s*$", RegexOptions.CultureInvariant | RegexOptions.Compiled)]
    private static partial Regex ActiveTagSearchRegex();
}


