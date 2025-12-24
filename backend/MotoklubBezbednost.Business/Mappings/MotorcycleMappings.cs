using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public static class MotorcycleMappings
{
    public static MotorcycleDto ToDto(this MotorcycleDb entity)
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
            RegisterPlate = entity.RegisterPlate
        };
    }

    public static IEnumerable<MotorcycleDto> ToDto(this IEnumerable<MotorcycleDb> entities)
        => entities.Select(e => e.ToDto());

    public static void UpdateEntity(this MotorcycleDb entity, MotorcycleDto dto)
    {
        entity.BrandName = dto.BrandName;
        entity.CommercialName = dto.CommercialName;
        entity.ModelName = dto.ModelName;
        entity.EngineDisplacment = dto.EngineDisplacment;
        entity.EnginePower = dto.EnginePower;
        entity.Color = dto.Color;
        entity.RegisterPlate = dto.RegisterPlate;
    }
}


