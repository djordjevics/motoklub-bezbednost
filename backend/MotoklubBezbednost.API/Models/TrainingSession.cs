using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.API.Models;

public class TrainingSession
{
    public int Id { get; set; }

    public DateTime? TheoryDate { get; set; }

    public DateTime? PolygonDate { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    public int? LevelId { get; set; }

    public int? Price { get; set; }

    [StringLength(500)]
    public string? Instructors { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    [ForeignKey("LevelId")]
    public Level? Level { get; set; }

    public ICollection<Training> Trainings { get; set; } = new List<Training>();
}

