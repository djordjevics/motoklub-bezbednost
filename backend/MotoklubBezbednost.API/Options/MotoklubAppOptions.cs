namespace MotoklubBezbednost.API.Options;

public class MotoklubAppOptions
{
    public const string SectionName = "Motoklub";

    public string DataDirectory { get; set; } = "data";
    public string SqliteFileName { get; set; } = "motoklub.db";
    public bool AutoMigrate { get; set; }

    public bool RequireAuthenticatedApi { get; set; } = true;
}
