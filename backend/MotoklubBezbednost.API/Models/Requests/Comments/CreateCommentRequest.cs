namespace MotoklubBezbednost.API.Models.Requests;

public class CreateCommentRequest
{
    public int MemberId { get; set; }
    public string? CommentText { get; set; }
}
