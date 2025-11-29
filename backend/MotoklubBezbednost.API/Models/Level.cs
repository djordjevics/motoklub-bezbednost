using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.API.Models;

public class Level
{
    public int Id { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    public ICollection<TrainingSession> TrainingSessions { get; set; } = new List<TrainingSession>();
}

