using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.API.Requests;

public class CreateTrainingSessionRequest
{
    [Required]
    public DateTime TheoryDate { get; set; }
    [Required]
    public DateTime PolygonDate { get; set; }
    [Required]
    public string City { get; set; }
    public int? Price { get; set; }
    public string? Instructors { get; set; }
    public string? Note { get; set; }
    [Required]
    public int LevelId { get; set; }
}


