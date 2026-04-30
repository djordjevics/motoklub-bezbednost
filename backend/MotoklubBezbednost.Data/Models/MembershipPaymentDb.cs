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

    // Foreign keys
    [Required]
    public int MemberId { get; set; }

    public int? PaymentTypeId { get; set; }

    // Navigation properties
    public virtual PaymentTypeDb? PaymentType { get; set; }
}


