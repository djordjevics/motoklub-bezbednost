namespace MotoklubBezbednost.Data.Models;

public abstract class DbModel
{
    public int Id { get; set; }

    public DateTime CreationTimestamp { get; set; }

    public DateTime? LastModificationTimestamp { get; set; }
}


