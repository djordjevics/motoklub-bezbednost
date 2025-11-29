using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.API.Models;

public class Comment
{
    public int Id { get; set; }

    [Required]
    public int MemberId { get; set; }

    public DateTime? CreationTime { get; set; }

    public DateTime? EditTime { get; set; }

    [StringLength(2000)]
    public string? CommentText { get; set; }

    // Navigation property
    [ForeignKey("MemberId")]
    public Member Member { get; set; } = null!;
}

