namespace MotoklubBezbednost.API.Models.Requests;

public class UpdateCommentRequest
{
    public int Id { get; set; }
    public string? CommentText { get; set; }
}
