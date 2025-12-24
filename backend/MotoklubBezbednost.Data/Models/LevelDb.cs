using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Data.Models;

public class LevelDb
{
    public int Id { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    public ICollection<TrainingSessionDb> TrainingSessions { get; set; } = new List<TrainingSessionDb>();
}


