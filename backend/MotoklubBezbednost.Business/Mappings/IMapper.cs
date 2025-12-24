namespace MotoklubBezbednost.Business.Mappings;

/// <summary>
/// Generic interface for one-way mapping from source to destination.
/// </summary>
/// <typeparam name="TSource">Source type.</typeparam>
/// <typeparam name="TDestination">Destination type.</typeparam>
public interface IOneWayMapper<TSource, TDestination>
{
    TDestination Map(TSource source);
}

/// <summary>
/// Generic interface for two-way mapping between two types.
/// </summary>
/// <typeparam name="TLeft">Left type.</typeparam>
/// <typeparam name="TRight">Right type.</typeparam>
public interface ITwoWayMapper<TLeft, TRight> : IOneWayMapper<TLeft, TRight>, IOneWayMapper<TRight, TLeft>
{
}

/// <summary>
/// Central mapper facade that resolves the appropriate typed mapper based on source and destination types.
/// </summary>
public interface IMapper
{
    TDestination Map<TSource, TDestination>(TSource source);
}

