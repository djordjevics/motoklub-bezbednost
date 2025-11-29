using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.API.Models;

public class Tag
{
    public int Id { get; set; }

    [Required]
    public int MemberId { get; set; }

    public int? TagNumber { get; set; }

    public DateTime? AssignedDate { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    // Navigation property
    [ForeignKey("MemberId")]
    public Member Member { get; set; } = null!;
}

