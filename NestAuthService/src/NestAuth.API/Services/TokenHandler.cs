namespace NestAuth.API.Services;

public class TokenHandler : ITokenHandler
{
    private readonly UserManager<AppUser> _userManager;

    public TokenHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<JwtTokenResponse> GenerateAccessTokenAsync(AppUser user)
    {
        var addMinutes = Configuration.GetConfiguratinValue<int>("IdentityParameters", "TokenExpirationInMinutes");

        JwtTokenResponse response = new()
        {
            RefreshToken = GenerateRefreshToken(),
            ExpiresIn = DateTime.Now.AddMinutes(addMinutes)
        };

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(Configuration.GetConfiguratinValue<string>("IdentityParameters", "SecurityKey")));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        JwtSecurityToken securityToken = new(
            audience: Configuration.GetConfiguratinValue<string>("IdentityParameters", "Audience"),
            issuer: Configuration.GetConfiguratinValue<string>("IdentityParameters", "Issuer"),
            expires: response.ExpiresIn,
            signingCredentials: signingCredentials,
            claims: claims
        );

        JwtSecurityTokenHandler tokenHandler = new();
        response.AccessToken = tokenHandler.WriteToken(securityToken);

        return response;
    }

    public string GenerateRefreshToken()
    {
        return $"{Guid.NewGuid()}-{Guid.NewGuid()}";
    }
}