using MediatR;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Cqrs.Levels.Queries;

public sealed class GetAllLevelsQuery : IRequest<IEnumerable<LevelDto>>
{
}
