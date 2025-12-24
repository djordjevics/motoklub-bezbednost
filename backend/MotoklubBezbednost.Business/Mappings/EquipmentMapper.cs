using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public sealed class EquipmentMapper : ITwoWayDbMapper<EquipmentDb, EquipmentDto>
{
    // IOneWayMapper<EquipmentDb, EquipmentDto>.Map
    public EquipmentDto Map(EquipmentDb source) => ToDto(source);

    // IOneWayMapper<EquipmentDto, EquipmentDb>.Map
    public EquipmentDb Map(EquipmentDto source) => ToEntity(source, null);

    // IDbToDtoMapper<EquipmentDb, EquipmentDto>
    public EquipmentDto ToDto(EquipmentDb entity)
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

    public IEnumerable<EquipmentDto> ToDto(IEnumerable<EquipmentDb> entities) => entities.Select(ToDto);

    // ITwoWayDbMapper<EquipmentDb, EquipmentDto>
    public EquipmentDb ToEntity(EquipmentDto dto, EquipmentDb? existing = null)
    {
        if (existing is null)
        {
            return new EquipmentDb
            {
                Pants = dto.Pants,
                Jacket = dto.Jacket,
                Vest = dto.Vest,
                WorkShirt = dto.WorkShirt,
                FormalShirt = dto.FormalShirt,
                Note = dto.Note
            };
        }

        existing.Pants = dto.Pants;
        existing.Jacket = dto.Jacket;
        existing.Vest = dto.Vest;
        existing.WorkShirt = dto.WorkShirt;
        existing.FormalShirt = dto.FormalShirt;
        existing.Note = dto.Note;
        return existing;
    }
}


