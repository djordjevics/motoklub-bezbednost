namespace MotoklubBezbednost.API.Models.Responses;

public class MembershipPaymentResponse
{
    public int Id { get; set; }
    public int? Amount { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int? PaymentForYear { get; set; }
    public string? Note { get; set; }
    public PaymentTypeResponse? PaymentType { get; set; }
}
