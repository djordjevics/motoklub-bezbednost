namespace MotoklubBezbednost.API.Models.Responses;

public class TagResponse
{
    public int Id { get; set; }
    public int? TagNumber { get; set; }
    public DateTime? AssignedDate { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public int MemberId { get; set; }
}
