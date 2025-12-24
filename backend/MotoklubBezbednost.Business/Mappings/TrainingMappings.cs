using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public static class TrainingMappings
{
    public static TrainingSessionDto ToDto(this TrainingSessionDb entity)
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
            Level = entity.Level?.ToDto()
        };
    }

    public static TrainingDto ToDto(this TrainingDb entity)
    {
        return new TrainingDto
        {
            Id = entity.Id,
            IsCertificateIssued = entity.IsCertificateIssued,
            Note = entity.Note,
            Member = entity.Member?.ToDto(),
            Motorcycle = entity.Motorcycle?.ToDto(),
            TrainingSession = entity.TrainingSession?.ToDto()
        };
    }

    public static IEnumerable<TrainingSessionDto> ToDto(this IEnumerable<TrainingSessionDb> entities)
        => entities.Select(e => e.ToDto());

    public static IEnumerable<TrainingDto> ToDto(this IEnumerable<TrainingDb> entities)
        => entities.Select(e => e.ToDto());
}


