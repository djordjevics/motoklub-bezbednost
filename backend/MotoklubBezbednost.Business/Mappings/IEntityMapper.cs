using System.Collections.Generic;

namespace MotoklubBezbednost.Business.Mappings;

/// <summary>
/// Generic interface for one-way mapping from a Db entity to a DTO.
/// </summary>
/// <typeparam name="TDb">Persistence/EF model type (e.g. MemberDb).</typeparam>
/// <typeparam name="TDto">Business DTO type (e.g. MemberDto).</typeparam>
public interface IDbToDtoMapper<TDb, TDto>
{
    TDto ToDto(TDb entity);
    IEnumerable<TDto> ToDto(IEnumerable<TDb> entities);
}

/// <summary>
/// Generic interface for two-way mapping between Db entity and DTO.
/// </summary>
/// <typeparam name="TDb">Persistence/EF model type (e.g. MemberDb).</typeparam>
/// <typeparam name="TDto">Business DTO type (e.g. MemberDto).</typeparam>
public interface ITwoWayDbMapper<TDb, TDto> : IDbToDtoMapper<TDb, TDto>
{
    /// <summary>
    /// Creates or updates a Db entity from the given DTO.
    /// </summary>
    /// <param name="dto">Source DTO.</param>
    /// <param name="existing">
    /// Optional existing entity to update; if null, a new instance should be created.
    /// </param>
    TDb ToEntity(TDto dto, TDb? existing = default);
}


