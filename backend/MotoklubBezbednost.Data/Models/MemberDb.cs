using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.Data.Models;

public class MemberDb : DbModel
{
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

    /// <summary>
    /// When true, this person is not treated as an active member (their record is kept for history only).
    /// Distinct from the age-based payment waiver for Aktiv members; see membership business rules at read-time.
    /// </summary>
    public bool MembershipExemptManual { get; set; }

    // Foreign keys
    public int? MemberTypeId { get; set; }

    // Navigation properties
    public virtual MemberTypeDb? MemberType { get; set; }

    public virtual ICollection<MotorcycleDb> Motorcycles { get; set; } = new List<MotorcycleDb>();
    public virtual EquipmentDb? Equipment { get; set; }
    public virtual ICollection<TrainingDb> Trainings { get; set; } = new List<TrainingDb>();
    public virtual ICollection<MembershipPaymentDb> MembershipPayments { get; set; } = new List<MembershipPaymentDb>();
    public virtual ICollection<CommentDb> Comments { get; set; } = new List<CommentDb>();
    public virtual ICollection<TagDb> Tags { get; set; } = new List<TagDb>();
}


