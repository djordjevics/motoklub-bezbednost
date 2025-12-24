using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public static class PaymentMappings
{
    public static PaymentTypeDto ToDto(this PaymentTypeDb entity)
    {
        return new PaymentTypeDto
        {
            Id = entity.Id,
            Type = entity.Type,
            Description = entity.Description
        };
    }

    public static MembershipPaymentDto ToDto(this MembershipPaymentDb entity)
    {
        return new MembershipPaymentDto
        {
            Id = entity.Id,
            Amount = entity.Amount,
            PaymentDate = entity.PaymentDate,
            PaymentForYear = entity.PaymentForYear,
            Note = entity.Note,
            PaymentType = entity.PaymentType?.ToDto()
        };
    }

    public static IEnumerable<PaymentTypeDto> ToDto(this IEnumerable<PaymentTypeDb> entities)
        => entities.Select(e => e.ToDto());

    public static IEnumerable<MembershipPaymentDto> ToDto(this IEnumerable<MembershipPaymentDb> entities)
        => entities.Select(e => e.ToDto());
}


