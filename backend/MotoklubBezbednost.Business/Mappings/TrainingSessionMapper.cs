using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public sealed class TrainingSessionMapper : ITwoWayDbMapper<TrainingSessionDb, TrainingSessionDto>
{
    // IOneWayMapper<TrainingSessionDb, TrainingSessionDto>.Map
    public TrainingSessionDto Map(TrainingSessionDb source) => ToDto(source);

    // IOneWayMapper<TrainingSessionDto, TrainingSessionDb>.Map
    public TrainingSessionDb Map(TrainingSessionDto source) => ToEntity(source, null);

    // IDbToDtoMapper<TrainingSessionDb, TrainingSessionDto>
    public TrainingSessionDto ToDto(TrainingSessionDb entity)
    {
        return new TrainingSessionDto
        {
            Id = entity.Id,
            TheoryDate = entity.TheoryDate,
            PolygonDate = entity.PolygonDate,
            City = entity.City,
            Price = entity.Price,
            Instructors = entity.Instructors,
            Note = entity.Note,
            Level = entity.Level != null ? new LevelDto
            {
                Id = entity.Level.Id,
                Name = entity.Level.Name,
                Note = entity.Level.Note
            } : null
        };
    }

    public IEnumerable<TrainingSessionDto> ToDto(IEnumerable<TrainingSessionDb> entities) => entities.Select(ToDto);

    // ITwoWayDbMapper<TrainingSessionDb, TrainingSessionDto>
    public TrainingSessionDb ToEntity(TrainingSessionDto dto, TrainingSessionDb? existing = null)
    {
        if (existing is null)
        {
            return new TrainingSessionDb
            {
                TheoryDate = dto.TheoryDate,
                PolygonDate = dto.PolygonDate,
                City = dto.City,
                Price = dto.Price,
                Instructors = dto.Instructors,
                Note = dto.Note
            };
        }

        existing.TheoryDate = dto.TheoryDate;
        existing.PolygonDate = dto.PolygonDate;
        existing.City = dto.City;
        existing.Price = dto.Price;
        existing.Instructors = dto.Instructors;
        existing.Note = dto.Note;
        return existing;
    }
}


