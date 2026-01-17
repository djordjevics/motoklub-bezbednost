using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Data.Models;

public class LevelDb : DbModel
{
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    public virtual ICollection<TrainingSessionDb> TrainingSessions { get; set; } = new List<TrainingSessionDb>();
}


