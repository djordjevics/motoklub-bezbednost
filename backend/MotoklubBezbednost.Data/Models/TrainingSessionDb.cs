using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoklubBezbednost.Data.Models;

public class TrainingSessionDb : DbModel
{
    public DateTime? TheoryDate { get; set; }

    public DateTime? PolygonDate { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    public int? Price { get; set; }

    [StringLength(500)]
    public string? Instructors { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    public LevelDb? Level { get; set; }

    public ICollection<TrainingDb> Trainings { get; set; } = new List<TrainingDb>();
}


