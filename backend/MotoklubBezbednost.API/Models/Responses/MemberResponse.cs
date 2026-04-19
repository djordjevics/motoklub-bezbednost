namespace MotoklubBezbednost.API.Models.Responses;

public class MemberResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? Jmbg { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Workplace { get; set; }
    public string? MobilePhone { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime? RegisteredOn { get; set; }
    public DateTime CreationTimestamp { get; set; }
    public DateTime? LastModificationTimestamp { get; set; }
    public string? Note { get; set; }
    public MemberTypeResponse? MemberType { get; set; }
    public ICollection<MotorcycleResponse> Motorcycles { get; set; } = new List<MotorcycleResponse>();
    public EquipmentResponse? Equipment { get; set; }
    public ICollection<TrainingResponse> Trainings { get; set; } = new List<TrainingResponse>();
    public ICollection<MembershipPaymentResponse> MembershipPayments { get; set; } = new List<MembershipPaymentResponse>();
    public ICollection<CommentResponse> Comments { get; set; } = new List<CommentResponse>();
    public ICollection<TagResponse> Tags { get; set; } = new List<TagResponse>();
}
