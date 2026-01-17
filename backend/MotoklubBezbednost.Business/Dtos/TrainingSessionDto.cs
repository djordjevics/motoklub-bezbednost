using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Business.Dtos;

public class TrainingSessionDto
{
    public int Id { get; set; }

    public DateTime? TheoryDate { get; set; }

    public DateTime? PolygonDate { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    public int? Price { get; set; }

    [StringLength(500)]
    public string? Instructors { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    public LevelDto? Level { get; set; }

    public DateTime CreationTimestamp { get; set; }

    public DateTime? LastModificationTimestamp { get; set; }
}


