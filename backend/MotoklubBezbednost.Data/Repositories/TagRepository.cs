using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public sealed class TagRepository : Repository<TagDb>, ITagRepository
{
    public TagRepository(ApplicationDbContext context) : base(context)
    {
    }
}

