using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Business.Dtos;

public class MembershipPaymentDto
{
    public int Id { get; set; }

    public int? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int? PaymentForYear { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    public int MemberId { get; set; }

    public int? PaymentTypeId { get; set; }

    public PaymentTypeDto? PaymentType { get; set; }
}


