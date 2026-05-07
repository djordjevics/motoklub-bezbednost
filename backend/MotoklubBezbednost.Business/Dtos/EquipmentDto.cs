using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Business.Dtos;

public class EquipmentDto
{
    public int Id { get; set; }

    public bool Pants { get; set; }

    public bool Jacket { get; set; }

    public bool Vest { get; set; }

    public bool WorkShirt { get; set; }

    public bool FormalShirt { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    public DateTime CreationTimestamp { get; set; }

    public DateTime? LastModificationTimestamp { get; set; }

    public int MemberId { get; set; }
}


