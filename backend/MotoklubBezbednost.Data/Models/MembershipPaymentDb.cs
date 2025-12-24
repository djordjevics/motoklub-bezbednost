using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.Data.Models;

public class MembershipPaymentDb : DbModel
{
    public int? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int? PaymentForYear { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    [Required]
    public MemberDb Member { get; set; } = null!;

    public PaymentTypeDb? PaymentType { get; set; }
}


