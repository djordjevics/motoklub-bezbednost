using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public sealed class LevelRepository : Repository<LevelDb>, ILevelRepository
{
    public LevelRepository(ApplicationDbContext context) : base(context)
    {
    }
}

