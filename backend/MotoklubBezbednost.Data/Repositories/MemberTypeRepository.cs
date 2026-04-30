using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public sealed class MemberTypeRepository : Repository<MemberTypeDb>, IMemberTypeRepository
{
    public MemberTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
}

