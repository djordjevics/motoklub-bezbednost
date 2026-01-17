using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public sealed class EquipmentMapper : ITwoWayMapper<EquipmentDb, EquipmentDto>
{
    public EquipmentDto Map(EquipmentDb source)
    {
        return new EquipmentDto
        {
            Id = source.Id,
            Pants = source.Pants,
            Jacket = source.Jacket,
            Vest = source.Vest,
            WorkShirt = source.WorkShirt,
            FormalShirt = source.FormalShirt,
            Note = source.Note,
            CreationTimestamp = source.CreationTimestamp,
            LastModificationTimestamp = source.LastModificationTimestamp
        };
    }

    public EquipmentDb Map(EquipmentDto source)
    {
        return new EquipmentDb
        {
            Pants = source.Pants,
            Jacket = source.Jacket,
            Vest = source.Vest,
            WorkShirt = source.WorkShirt,
            FormalShirt = source.FormalShirt,
            Note = source.Note
        };
    }
}


