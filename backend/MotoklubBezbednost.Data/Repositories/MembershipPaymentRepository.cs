using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public sealed class MembershipPaymentRepository : Repository<MembershipPaymentDb>, IMembershipPaymentRepository
{
    public MembershipPaymentRepository(ApplicationDbContext context) : base(context)
    {
    }
}

