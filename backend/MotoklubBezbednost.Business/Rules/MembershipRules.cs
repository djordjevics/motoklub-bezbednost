using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Rules;

/// <summary>
/// Membership payment and roster rules. Aktiv 65+ payment waiver is derived at read-time.
/// <see cref="MemberDb.MembershipExemptManual"/> means not an active member while keeping the record.
/// </summary>
public static class MembershipRules
{
    /// <summary>The MemberType.Id of "Aktiv". Hardcoded because the row is seeded with a fixed Id (see SeedLookupValues).</summary>
    public const int AktivMemberTypeId = 2;

    /// <summary>Age (inclusive) at which an Aktiv member stops paying membership.</summary>
    public const int AktivExemptAge = 65;

    /// <summary>
    /// Returns true when the member is exempt from paying membership *because of the age override*.
    /// Note: this is NOT the same as "doesn't pay" — types 3/4 also don't pay, but that is governed by
    /// MemberType.PaidMembership, not by this rule. Callers that need the combined view should use
    /// (memberType.PaidMembership AND NOT IsAktivAgedOutOfMembership(...)).
    /// </summary>
    public static bool IsAktivAgedOutOfMembership(MemberDb member, DateTime today)
    {
        if (member.MemberTypeId != AktivMemberTypeId) return false;
        if (member.DateOfBirth is not { } dob) return false;
        return AgeAt(dob, today) >= AktivExemptAge;
    }

    public static int AgeAt(DateTime dob, DateTime today)
    {
        var age = today.Year - dob.Year;
        if (today < dob.AddYears(age)) age--;
        return age;
    }

    /// <summary>
    /// Sets <see cref="MemberDto.MembershipExemptManual"/> (from entity),
    /// <see cref="MemberDto.IsMembershipPaymentExemptDueToAge"/> (Aktiv 65+ payment rule),
    /// and <see cref="MemberDto.IsMembershipPaymentRequired"/> (combined: group + age + inactive override).
    /// </summary>
    public static void EnrichMembershipExempt(MemberDb entity, MemberDto dto, DateTime today)
    {
        dto.MembershipExemptManual = entity.MembershipExemptManual;

        var ageExempt = IsAktivAgedOutOfMembership(entity, today);
        dto.IsMembershipPaymentExemptDueToAge = ageExempt;

        var groupRequiresPayment = dto.MemberType?.PaidMembership ?? false;
        dto.IsMembershipPaymentRequired = !entity.MembershipExemptManual && groupRequiresPayment && !ageExempt;
    }
}
