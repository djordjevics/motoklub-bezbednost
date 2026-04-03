namespace MotoklubBezbednost.API.Models.Responses;

public class EquipmentResponse
{
    public int Id { get; set; }
    public bool Pants { get; set; }
    public bool Jacket { get; set; }
    public bool Vest { get; set; }
    public bool WorkShirt { get; set; }
    public bool FormalShirt { get; set; }
    public string? Note { get; set; }
    public DateTime CreationTimestamp { get; set; }
    public DateTime? LastModificationTimestamp { get; set; }
}
