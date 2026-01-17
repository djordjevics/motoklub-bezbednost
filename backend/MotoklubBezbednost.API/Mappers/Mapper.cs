using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MotoklubBezbednost.Business.Mappings;

namespace MotoklubBezbednost.API.Mappers;

/// <summary>
/// Simple mapper implementation that resolves typed mappers from DI container.
/// Provides unified mapping interface for Db &lt;-&gt; Dto conversions.
/// </summary>
public sealed class Mapper : IMapper
{
    private readonly IServiceProvider _serviceProvider;

    public Mapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        // Try to resolve as ITwoWayMapper in the requested direction (for Db <-> Dto mappings)
        var twoWayMapper = _serviceProvider.GetService<ITwoWayMapper<TSource, TDestination>>();
        if (twoWayMapper != null)
        {
            return twoWayMapper.Map(source);
        }

        // Try reverse direction (since ITwoWayMapper<A, B> and ITwoWayMapper<B, A> are different types)
        var reverseMapperType = typeof(ITwoWayMapper<,>).MakeGenericType(typeof(TDestination), typeof(TSource));
        var reverseMapper = _serviceProvider.GetService(reverseMapperType);
        if (reverseMapper != null)
        {
            // Call the Map method that takes TSource and returns TDestination
            var mapMethod = reverseMapperType.GetMethod("Map", new[] { typeof(TSource) });
            if (mapMethod != null)
            {
                return (TDestination)mapMethod.Invoke(reverseMapper, new object[] { source! })!;
            }
        }

        throw new InvalidOperationException($"No mapper registered for {typeof(TSource).Name} -> {typeof(TDestination).Name}");
    }
}

