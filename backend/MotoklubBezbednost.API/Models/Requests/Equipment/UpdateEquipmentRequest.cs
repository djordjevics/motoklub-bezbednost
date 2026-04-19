namespace MotoklubBezbednost.API.Models.Requests;

public class UpdateEquipmentRequest
{
    public int Id { get; set; }
    public bool Pants { get; set; }
    public bool Jacket { get; set; }
    public bool Vest { get; set; }
    public bool WorkShirt { get; set; }
    public bool FormalShirt { get; set; }
    public string? Note { get; set; }
}


