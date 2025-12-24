using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public sealed class MotorcycleMapper : ITwoWayDbMapper<MotorcycleDb, MotorcycleDto>
{
    // IOneWayMapper<MotorcycleDb, MotorcycleDto>.Map
    public MotorcycleDto Map(MotorcycleDb source) => ToDto(source);

    // IOneWayMapper<MotorcycleDto, MotorcycleDb>.Map
    public MotorcycleDb Map(MotorcycleDto source) => ToEntity(source, null);

    // IDbToDtoMapper<MotorcycleDb, MotorcycleDto>
    public MotorcycleDto ToDto(MotorcycleDb entity)
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

    public IEnumerable<MotorcycleDto> ToDto(IEnumerable<MotorcycleDb> entities) => entities.Select(ToDto);

    // ITwoWayDbMapper<MotorcycleDb, MotorcycleDto>
    public MotorcycleDb ToEntity(MotorcycleDto dto, MotorcycleDb? existing = null)
    {
        if (existing is null)
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

        existing.BrandName = dto.BrandName;
        existing.CommercialName = dto.CommercialName;
        existing.ModelName = dto.ModelName;
        existing.EngineDisplacment = dto.EngineDisplacment;
        existing.EnginePower = dto.EnginePower;
        existing.Color = dto.Color;
        existing.RegisterPlate = dto.RegisterPlate;
        return existing;
    }
}


