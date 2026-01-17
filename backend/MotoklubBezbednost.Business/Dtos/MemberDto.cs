using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Business.Dtos;

public class MemberDto
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

    public DateTime CreationTimestamp { get; set; }

    public DateTime? LastModificationTimestamp { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties (DTO graph)
    public MemberTypeDto? MemberType { get; set; }

    public ICollection<MotorcycleDto> Motorcycles { get; set; } = new List<MotorcycleDto>();
    public EquipmentDto? Equipment { get; set; }
    public ICollection<TrainingDto> Trainings { get; set; } = new List<TrainingDto>();
    public ICollection<MembershipPaymentDto> MembershipPayments { get; set; } = new List<MembershipPaymentDto>();
    public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
    public ICollection<TagDto> Tags { get; set; } = new List<TagDto>();
}


