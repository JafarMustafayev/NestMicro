namespace NestAuth.API.ConfigurationModels;

public class IdentityParameters
{
    public string SecurityKey { get; set; } = string.Empty;
    public JwtConfiguration JwtConfiguration { get; set; } = new();
    public Expiration Expiration { get; set; } = new();
}

public class JwtConfiguration
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}

public class Expiration
{
    public TimeOnly AccessTokenExpiration { get; set; }
    public TimeOnly RefreshTokenExpiration { get; set; }
}