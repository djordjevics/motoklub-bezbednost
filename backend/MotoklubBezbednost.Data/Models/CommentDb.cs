using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.Data.Models;

public class CommentDb
{
    public int Id { get; set; }

    public DateTime? CreationTime { get; set; }

    public DateTime? EditTime { get; set; }

    [StringLength(2000)]
    public string? CommentText { get; set; }

    // Navigation property
    [Required]
    public MemberDb Member { get; set; } = null!;
}


