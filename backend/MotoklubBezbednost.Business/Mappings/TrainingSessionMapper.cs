using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public sealed class TrainingSessionMapper : ITwoWayMapper<TrainingSessionDb, TrainingSessionDto>
{
    public TrainingSessionDto Map(TrainingSessionDb entity)
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
            CreationTimestamp = entity.CreationTimestamp,
            LastModificationTimestamp = entity.LastModificationTimestamp,
            Level = entity.Level != null ? new LevelDto
            {
                Id = entity.Level.Id,
                Name = entity.Level.Name,
                Note = entity.Level.Note
            } : null
        };
    }

    public TrainingSessionDb Map(TrainingSessionDto dto)
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
}


