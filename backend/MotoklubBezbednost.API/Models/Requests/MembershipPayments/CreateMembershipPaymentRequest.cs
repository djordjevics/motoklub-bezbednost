namespace MotoklubBezbednost.API.Models.Requests;

public class CreateMembershipPaymentRequest
{
    public int MemberId { get; set; }
    public int? Amount { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int? PaymentForYear { get; set; }
    public int? PaymentTypeId { get; set; }
    public string? Note { get; set; }
}
