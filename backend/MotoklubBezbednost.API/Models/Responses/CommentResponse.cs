namespace MotoklubBezbednost.API.Models.Responses;

public class CommentResponse
{
    public int Id { get; set; }
    public DateTime? CreationTime { get; set; }
    public DateTime? EditTime { get; set; }
    public string? CommentText { get; set; }
    public int MemberId { get; set; }
}
