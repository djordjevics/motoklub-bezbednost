using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.Data.Models;

public class CommentDb : DbModel
{
    public DateTime? CreationTime { get; set; }

    public DateTime? EditTime { get; set; }

    [StringLength(2000)]
    public string? CommentText { get; set; }

    // Foreign keys
    [Required]
    public int MemberId { get; set; }

    // Navigation properties
}


