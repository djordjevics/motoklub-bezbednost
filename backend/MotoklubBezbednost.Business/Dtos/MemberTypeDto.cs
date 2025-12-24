using System.ComponentModel.DataAnnotations;

namespace MotoklubBezbednost.Business.Dtos;

public class MemberTypeDto
{
    public int Id { get; set; }

    public int? Prefix { get; set; }

    [StringLength(100)]
    public string? TypeName { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    public bool PaidMembership { get; set; }
}


