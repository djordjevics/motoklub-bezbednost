using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Data.Models;

public class MemberTypeDb : DbModel
{
    public int? Prefix { get; set; }

    [StringLength(100)]
    public string? TypeName { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    public bool PaidMembership { get; set; }

    // Navigation properties
    public virtual ICollection<MemberDb> Members { get; set; } = new List<MemberDb>();
}


