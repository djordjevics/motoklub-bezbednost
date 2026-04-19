namespace MotoklubBezbednost.API.Models.Requests;

public class UpdateTrainingSessionRequest
{
    public int Id { get; set; }
    public DateTime? TheoryDate { get; set; }
    public DateTime? PolygonDate { get; set; }
    public string? City { get; set; }
    public int? Price { get; set; }
    public string? Instructors { get; set; }
    public string? Note { get; set; }
    public int? LevelId { get; set; }
}


