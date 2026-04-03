namespace MotoklubBezbednost.API.Models.Responses;

public class MemberTypeResponse
{
    public int Id { get; set; }
    public int? Prefix { get; set; }
    public string? TypeName { get; set; }
    public string? Color { get; set; }
    public bool PaidMembership { get; set; }
}
