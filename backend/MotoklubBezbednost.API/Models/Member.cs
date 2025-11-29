using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.API.Models;

public class Member
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Surname { get; set; } = string.Empty;

    [StringLength(13)]
    public string? Jmbg { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(200)]
    public string? Workplace { get; set; }

    [StringLength(20)]
    public string? MobilePhone { get; set; }

    [StringLength(100)]
    public string? EmergencyContact { get; set; }

    [StringLength(20)]
    public string? EmergencyContactPhone { get; set; }

    [StringLength(200)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    public DateTime? RegisteredOn { get; set; }

    public int? MemberTypeId { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    [ForeignKey("MemberTypeId")]
    public MemberType? MemberType { get; set; }

    public ICollection<Motorcycle> Motorcycles { get; set; } = new List<Motorcycle>();
    public Equipment? Equipment { get; set; }
    public ICollection<Training> Trainings { get; set; } = new List<Training>();
    public ICollection<MembershipPayment> MembershipPayments { get; set; } = new List<MembershipPayment>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}

