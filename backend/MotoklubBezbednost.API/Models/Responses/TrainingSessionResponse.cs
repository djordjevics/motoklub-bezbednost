namespace MotoklubBezbednost.API.Models.Responses;

public class TrainingSessionResponse
{
    public int Id { get; set; }
    public DateTime? TheoryDate { get; set; }
    public DateTime? PolygonDate { get; set; }
    public string? City { get; set; }
    public int? Price { get; set; }
    public string? Instructors { get; set; }
    public string? Note { get; set; }
    public LevelResponse? Level { get; set; }
    public DateTime CreationTimestamp { get; set; }
    public DateTime? LastModificationTimestamp { get; set; }
}
