using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public static class EquipmentMappings
{
    public static EquipmentDto ToDto(this EquipmentDb entity)
    {
        return new EquipmentDto
        {
            Id = entity.Id,
            Pants = entity.Pants,
            Jacket = entity.Jacket,
            Vest = entity.Vest,
            WorkShirt = entity.WorkShirt,
            FormalShirt = entity.FormalShirt,
            Note = entity.Note
        };
    }

    public static IEnumerable<EquipmentDto> ToDto(this IEnumerable<EquipmentDb> entities)
        => entities.Select(e => e.ToDto());

    public static void UpdateEntity(this EquipmentDb entity, EquipmentDto dto)
    {
        entity.Pants = dto.Pants;
        entity.Jacket = dto.Jacket;
        entity.Vest = dto.Vest;
        entity.WorkShirt = dto.WorkShirt;
        entity.FormalShirt = dto.FormalShirt;
        entity.Note = dto.Note;
    }
}


