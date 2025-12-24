using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Business.Dtos;

public class LevelDto
{
    public int Id { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }
}


