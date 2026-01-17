using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public sealed class TrainingMapper : ITwoWayMapper<TrainingDb, TrainingDto>
{
    public TrainingDto Map(TrainingDb entity)
    {
        return new TrainingDto
        {
            Id = entity.Id,
            IsCertificateIssued = entity.IsCertificateIssued,
            Note = entity.Note,
            CreationTimestamp = entity.CreationTimestamp,
            LastModificationTimestamp = entity.LastModificationTimestamp,
            // Navigation properties left null to avoid circular references
            // These should be loaded via Include() in queries if needed
            Member = null,
            Motorcycle = null,
            TrainingSession = null
        };
    }

    public TrainingDb Map(TrainingDto dto)
    {
        return new TrainingDb
        {
            IsCertificateIssued = dto.IsCertificateIssued,
            Note = dto.Note
        };
    }
}


