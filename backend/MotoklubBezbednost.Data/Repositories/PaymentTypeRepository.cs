using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Repositories;

public sealed class PaymentTypeRepository : Repository<PaymentTypeDb>, IPaymentTypeRepository
{
    public PaymentTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
}

