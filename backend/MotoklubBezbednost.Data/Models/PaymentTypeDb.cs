using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Data.Models;

public class PaymentTypeDb
{
    public int Id { get; set; }

    [StringLength(100)]
    public string? Type { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<MembershipPaymentDb> MembershipPayments { get; set; } = new List<MembershipPaymentDb>();
}


