using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public sealed class CommentRepository : Repository<CommentDb>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context)
    {
    }
}

