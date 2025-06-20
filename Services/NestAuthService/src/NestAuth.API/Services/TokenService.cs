using System.Security.Cryptography;

namespace NestAuth.API.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenRepository _tokenRepository;

    private readonly IUserDeviceInfoService _userDeviceInfoService;

    public TokenService(
        UserManager<AppUser> userManager,
        ITokenRepository tokenRepository,
        IUserDeviceInfoService userDeviceInfoService)
    {
        //_userManager = userManager;
        _tokenRepository = tokenRepository;
        _userDeviceInfoService = userDeviceInfoService;
        _userManager = userManager;
    }

    public async Task<JwtTokenResponse> GenerateAccessTokenAsync(AppUser user, string sessionId)
    {
        var rft = await GenerateRefreshTokenAsync(user.Id, sessionId);

        JwtTokenResponse response = new()
        {
            RefreshToken = rft.Token,
            ExpiresIn = DateTime.UtcNow.AddMinutes(Configurations.GetConfiguration<IdentityParameters>().Expiration.AccessTokenExpiration.Minute),
            IssuedAt = rft.CreatedAt
        };

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(Configurations.GetConfiguration<IdentityParameters>().SecurityKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? "Email"),
            new(ClaimTypes.Name, user.UserName ?? "UserName")
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        JwtSecurityToken securityToken = new(
            audience: Configurations.GetConfiguration<IdentityParameters>().JwtConfiguration.Audience,
            issuer: Configurations.GetConfiguration<IdentityParameters>().JwtConfiguration.Issuer,
            expires: response.ExpiresIn,
            signingCredentials: signingCredentials,
            claims: claims
        );

        JwtSecurityTokenHandler tokenHandler = new();
        response.AccessToken = tokenHandler.WriteToken(securityToken);

        return response;
    }

    public async Task<UserRefreshToken> GenerateRefreshTokenAsync(string userId, string sessionId)
    {
        UserRefreshToken refreshToken = new()
        {
            UserId = userId,
            Expires = DateTime.UtcNow.AddMinutes(Configurations.GetConfiguration<IdentityParameters>().Expiration.AccessTokenExpiration.Minute),
            CreatedByIp = _userDeviceInfoService.GetClientIp(),
            SessionId = sessionId
        };

        await _tokenRepository.AddAsync(refreshToken);
        await _tokenRepository.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<string> GetSessionId(string userId, string refreshToken)
    {
        var token = await _tokenRepository.GetByExpressionAsync(x => x.UserId == userId && x.Token == refreshToken);
        if (token == null)
        {
            throw new AuthenticationException("Invalid or expired refresh token.");
        }

        return token.SessionId;
    }

    public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
    {
        var result = await _tokenRepository.AnyAsync(x => x.UserId == userId &&
                                                          x.Token == refreshToken &&
                                                          !x.IsUsed && !x.IsRevoked && x.Expires > DateTime.UtcNow);

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
        token.Expires = DateTime.UtcNow;
        _tokenRepository.Update(token);
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
        token.RevokedByIp = _userDeviceInfoService.GetClientIp();
        _tokenRepository.Update(token);
        await _tokenRepository.SaveChangesAsync();
    }

    public async Task RevokeUserRefreshAllTokens(string userId)
    {
        var tokens = _tokenRepository.GetAllByExpression(x => x.UserId == userId && !x.IsRevoked && x.Expires > DateTime.UtcNow);
        if (tokens.Count > 0)
        {
            foreach (var token in tokens.Items)
            {
                if (token != null)
                {
                    token.IsRevoked = true;
                    token.RevokedByIp = _userDeviceInfoService.GetClientIp();
                    _tokenRepository.Update(token);
                }
            }
        }

        await _tokenRepository.SaveChangesAsync();
    }

    public string GenerateOtpToken(int length = 6)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        var num = BitConverter.ToInt32(bytes) & 0x7FFFFFFF;
        return num.ToString().PadLeft(length, '0').Substring(0, length);
    }

    public string GenerateTemporaryToken()
    {
        return $"{Guid.NewGuid().ToString()}-{Guid.NewGuid().ToString()}";
    }
}