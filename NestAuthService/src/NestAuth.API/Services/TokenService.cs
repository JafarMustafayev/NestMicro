namespace NestAuth.API.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenRepository _tokenRepository;

    public TokenService(
        UserManager<AppUser> userManager,
        ITokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;
    }

    public async Task<JwtTokenResponse> GenerateAccessTokenAsync(AppUser user)
    {
        var addMinutes = Configuration.GetConfiguratinValue<int>("IdentityParameters", "TokenExpirationInMinutes");

        var rft = await GenerateRefreshTokenAsync(user.Id);

        JwtTokenResponse response = new()
        {
            RefreshToken = rft.Token,
            ExpiresIn = DateTime.Now.AddMinutes(addMinutes),
            IssuedAt = rft.CreatedAt,
        };

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(Configuration.GetConfiguratinValue<string>("IdentityParameters", "SecurityKey")));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
        };

        //var roles = await _userManager.GetRolesAsync(user);

        //foreach (var role in roles)
        //{
        //    claims.Add(new Claim(ClaimTypes.Role, role));
        //}

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

    public async Task<UserRefreshToken> GenerateRefreshTokenAsync(string userId)
    {
        UserRefreshToken refreshToken = new()
        {
            UserId = userId,
            Token = $"{Guid.NewGuid()}-{Guid.NewGuid()}",
            Expires = DateTime.UtcNow.AddMinutes(Configuration.GetConfiguratinValue<int>("IdentityParameters", "RefreshTokenExpirationInMinutes")),
            IsUsed = false,
            IsRevoked = false,
            CreatedByIp = string.Empty,
            RevokedByIp = string.Empty
        };

        await _tokenRepository.AddAsync(refreshToken);
        await _tokenRepository.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
    {
        var result = await _tokenRepository.AnyAsync(
            x => x.UserId == userId &&
            x.Token == refreshToken &&
            (!x.IsUsed && !x.IsRevoked && x.Expires > DateTime.UtcNow));

        return result;
    }

    public async Task MarkRefreshTokenAsUsedAsync(string userId, string refreshToken)
    {
        var token = await _tokenRepository.GetByExpressionAsync(x => x.UserId == userId && x.Token == refreshToken);

        if (token == null || token.Expires < DateTime.UtcNow)
        {
            throw new AuthenticationException("Invalid or expired refresh token.");
        }

        token.IsUsed = true;
        await _tokenRepository.SaveChangesAsync();
    }

    public async Task RevokeRefreshTokenAsync(string userId, string refreshToken)
    {
        var token = await _tokenRepository.GetByExpressionAsync(x => x.UserId == userId && x.Token == refreshToken);

        if (token == null || token.Expires < DateTime.UtcNow)
        {
            throw new AuthenticationException("Invalid or expired refresh token.");
        }

        token.IsRevoked = true;
        await _tokenRepository.SaveChangesAsync();
    }
}