using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.Data.Models;

public class MemberDb
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

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    public MemberTypeDb? MemberType { get; set; }

    public ICollection<MotorcycleDb> Motorcycles { get; set; } = new List<MotorcycleDb>();
    public EquipmentDb? Equipment { get; set; }
    public ICollection<TrainingDb> Trainings { get; set; } = new List<TrainingDb>();
    public ICollection<MembershipPaymentDb> MembershipPayments { get; set; } = new List<MembershipPaymentDb>();
    public ICollection<CommentDb> Comments { get; set; } = new List<CommentDb>();
    public ICollection<TagDb> Tags { get; set; } = new List<TagDb>();
}


