using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public sealed class MotorcycleMapper : ITwoWayMapper<MotorcycleDb, MotorcycleDto>
{
    public MotorcycleDto Map(MotorcycleDb entity)
    {
        return new MotorcycleDto
        {
            Id = entity.Id,
            BrandName = entity.BrandName,
            CommercialName = entity.CommercialName,
            ModelName = entity.ModelName,
            EngineDisplacment = entity.EngineDisplacment,
            EnginePower = entity.EnginePower,
            Color = entity.Color,
            RegisterPlate = entity.RegisterPlate,
            CreationTimestamp = entity.CreationTimestamp,
            LastModificationTimestamp = entity.LastModificationTimestamp
        };
    }

    public MotorcycleDb Map(MotorcycleDto dto)
    {
        return new MotorcycleDb
        {
            BrandName = dto.BrandName,
            CommercialName = dto.CommercialName,
            ModelName = dto.ModelName,
            EngineDisplacment = dto.EngineDisplacment,
            EnginePower = dto.EnginePower,
            Color = dto.Color,
            RegisterPlate = dto.RegisterPlate
        };
    }
}


