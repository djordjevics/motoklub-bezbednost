using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.API.Models;

public class MembershipPayment
{
    public int Id { get; set; }

    [Required]
    public int MemberId { get; set; }

    public int? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int? PaymentForYear { get; set; }

    public int? PaymentTypeId { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    [ForeignKey("MemberId")]
    public Member Member { get; set; } = null!;

    [ForeignKey("PaymentTypeId")]
    public PaymentType? PaymentType { get; set; }
}

