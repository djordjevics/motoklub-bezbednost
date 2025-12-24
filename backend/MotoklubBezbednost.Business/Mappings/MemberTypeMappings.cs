using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public static class MemberTypeMappings
{
    public static MemberTypeDto ToDto(this MemberTypeDb entity)
    {
        return new MemberTypeDto
        {
            Id = entity.Id,
            Prefix = entity.Prefix,
            TypeName = entity.TypeName,
            Color = entity.Color,
            PaidMembership = entity.PaidMembership
        };
    }

    public static IEnumerable<MemberTypeDto> ToDto(this IEnumerable<MemberTypeDb> entities)
        => entities.Select(e => e.ToDto());
}


