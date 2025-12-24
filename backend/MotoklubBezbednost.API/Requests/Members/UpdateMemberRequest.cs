using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.API.Requests;

public class UpdateMemberRequest
{
    [Required]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Jmbg { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Workplace { get; set; }
    public string? MobilePhone { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
}


