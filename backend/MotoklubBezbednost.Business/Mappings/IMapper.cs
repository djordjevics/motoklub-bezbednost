namespace MotoklubBezbednost.Business.Mappings;

/// <summary>
/// Generic interface for two-way mapping between two types (for Db <-> Dto).
/// </summary>
/// <typeparam name="TLeft">Left type.</typeparam>
/// <typeparam name="TRight">Right type.</typeparam>
public interface ITwoWayMapper<TLeft, TRight>
{
    TRight Map(TLeft source);
    TLeft Map(TRight source);
}

/// <summary>
/// Simple mapper interface for mapping between types.
/// </summary>
public interface IMapper
{
    TDestination Map<TSource, TDestination>(TSource source);
}

