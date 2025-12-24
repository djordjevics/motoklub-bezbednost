using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public sealed class TrainingMapper : ITwoWayDbMapper<TrainingDb, TrainingDto>
{
    // IOneWayMapper<TrainingDb, TrainingDto>.Map
    public TrainingDto Map(TrainingDb source) => ToDto(source);

    // IOneWayMapper<TrainingDto, TrainingDb>.Map
    public TrainingDb Map(TrainingDto source) => ToEntity(source, null);

    // IDbToDtoMapper<TrainingDb, TrainingDto>
    public TrainingDto ToDto(TrainingDb entity)
    {
        return new TrainingDto
        {
            Id = entity.Id,
            IsCertificateIssued = entity.IsCertificateIssued,
            Note = entity.Note,
            // Navigation properties left null to avoid circular references
            // These should be loaded via Include() in queries if needed
            Member = null,
            Motorcycle = null,
            TrainingSession = null
        };
    }

    public IEnumerable<TrainingDto> ToDto(IEnumerable<TrainingDb> entities) => entities.Select(ToDto);

    // ITwoWayDbMapper<TrainingDb, TrainingDto>
    public TrainingDb ToEntity(TrainingDto dto, TrainingDb? existing = null)
    {
        if (existing is null)
        {
            return new TrainingDb
            {
                IsCertificateIssued = dto.IsCertificateIssued,
                Note = dto.Note
            };
        }

        existing.IsCertificateIssued = dto.IsCertificateIssued;
        existing.Note = dto.Note;
        return existing;
    }
}


