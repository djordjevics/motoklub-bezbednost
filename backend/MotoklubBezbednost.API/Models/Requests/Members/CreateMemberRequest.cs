using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.API.Models.Requests;

public class CreateMemberRequest
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    public string? Jmbg { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Workplace { get; set; }
    [Required]
    public string MobilePhone { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyContactPhone { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Address { get; set; }
    public string? Note { get; set; }
}


