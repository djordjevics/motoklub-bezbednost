using Microsoft.Extensions.DependencyInjection;
using MotoklubBezbednost.Business.Mappings;

namespace MotoklubBezbednost.API.Mappers;

/// <summary>
/// Central mapper facade implementation that resolves typed mappers from DI container.
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
        var typedMapper = _serviceProvider.GetRequiredService<IOneWayMapper<TSource, TDestination>>();
        return typedMapper.Map(source);
    }
}

