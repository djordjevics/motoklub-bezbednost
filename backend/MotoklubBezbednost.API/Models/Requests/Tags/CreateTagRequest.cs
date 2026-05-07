namespace MotoklubBezbednost.API.Models.Requests;

public class CreateTagRequest
{
    public int MemberId { get; set; }
    public int? TagNumber { get; set; }
    public DateTime? AssignedDate { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}
