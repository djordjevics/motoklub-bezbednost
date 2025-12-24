using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.Data.Models;

public class TagDb
{
    public int Id { get; set; }

    public int? TagNumber { get; set; }

    public DateTime? AssignedDate { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    // Navigation property
    [Required]
    public MemberDb Member { get; set; } = null!;
}


