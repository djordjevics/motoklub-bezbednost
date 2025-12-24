using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public static class CommentAndTagMappings
{
    public static CommentDto ToDto(this CommentDb entity)
    {
        return new CommentDto
        {
            Id = entity.Id,
            CreationTime = entity.CreationTime,
            EditTime = entity.EditTime,
            CommentText = entity.CommentText
        };
    }

    public static TagDto ToDto(this TagDb entity)
    {
        return new TagDto
        {
            Id = entity.Id,
            TagNumber = entity.TagNumber,
            AssignedDate = entity.AssignedDate,
            ValidFrom = entity.ValidFrom,
            ValidTo = entity.ValidTo
        };
    }

    public static IEnumerable<CommentDto> ToDto(this IEnumerable<CommentDb> entities)
        => entities.Select(e => e.ToDto());

    public static IEnumerable<TagDto> ToDto(this IEnumerable<TagDb> entities)
        => entities.Select(e => e.ToDto());
}


