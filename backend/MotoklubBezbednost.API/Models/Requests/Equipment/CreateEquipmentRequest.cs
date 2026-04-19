namespace MotoklubBezbednost.API.Models.Requests;

public class CreateEquipmentRequest
{
    public bool Pants { get; set; }
    public bool Jacket { get; set; }
    public bool Vest { get; set; }
    public bool WorkShirt { get; set; }
    public bool FormalShirt { get; set; }
    public string? Note { get; set; }
    public int MemberId { get; set; }
}


