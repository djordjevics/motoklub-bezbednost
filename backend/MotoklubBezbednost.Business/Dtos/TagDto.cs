using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Business.Dtos;

public class TagDto
{
    public int Id { get; set; }

    public int? TagNumber { get; set; }

    public DateTime? AssignedDate { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }
}


