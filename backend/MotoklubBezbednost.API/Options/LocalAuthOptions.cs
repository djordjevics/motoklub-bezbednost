namespace MotoklubBezbednost.API.Options;

public class LocalAuthOptions
{
    public const string SectionName = "LocalAuth";

    public string Username { get; set; } = "admin";
    public string Password { get; set; } = "change-me";
}
