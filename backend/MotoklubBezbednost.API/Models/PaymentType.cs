using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.API.Models;

public class PaymentType
{
    public int Id { get; set; }

    [StringLength(100)]
    public string? Type { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<MembershipPayment> MembershipPayments { get; set; } = new List<MembershipPayment>();
}

