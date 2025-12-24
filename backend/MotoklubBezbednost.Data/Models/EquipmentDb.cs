using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.Data.Models;

public class EquipmentDb
{
    public int Id { get; set; }

    public bool Pants { get; set; }

    public bool Jacket { get; set; }

    public bool Vest { get; set; }

    public bool WorkShirt { get; set; }

    public bool FormalShirt { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation property
    [Required]
    public MemberDb Member { get; set; } = null!;
}


