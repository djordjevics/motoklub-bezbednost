namespace MotoklubBezbednost.API.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "MotoklubBezbednost";
    public string Audience { get; set; } = "MotoklubBezbednost.App";
    public string SigningKey { get; set; } = "";
    public int ExpiryHours { get; set; } = 12;
}
