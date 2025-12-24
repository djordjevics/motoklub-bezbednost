using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public static class LevelMappings
{
    public static LevelDto ToDto(this LevelDb entity)
    {
        return new LevelDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Note = entity.Note
        };
    }

    public static IEnumerable<LevelDto> ToDto(this IEnumerable<LevelDb> entities)
        => entities.Select(e => e.ToDto());
}


