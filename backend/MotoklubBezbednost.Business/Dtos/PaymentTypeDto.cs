using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Business.Dtos;

public class PaymentTypeDto
{
    public int Id { get; set; }

    [StringLength(100)]
    public string? Type { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }
}


