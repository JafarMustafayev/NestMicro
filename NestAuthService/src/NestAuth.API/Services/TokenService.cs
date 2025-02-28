namespace NestAuth.API.Services;

public class TokenService : ITokenService
{
    //private readonly UserManager<AppUser> _userManager;
    private readonly ITokenRepository _tokenRepository;

    private readonly IUserDeviceInfoService _userDeviceInfoService;

    public TokenService(

        //UserManager<AppUser> userManager,
        ITokenRepository tokenRepository,
        IUserDeviceInfoService userDeviceInfoService)
    {
        //_userManager = userManager;
        _tokenRepository = tokenRepository;
        _userDeviceInfoService = userDeviceInfoService;
    }

    public async Task<JwtTokenResponse> GenerateAccessTokenAsync(AppUser user, string sessionId)
    {
        var addMinutes = Configurations.GetConfiguratinValue<int>("IdentityParameters", "TokenExpirationInMinutes");

        var rft = await GenerateRefreshTokenAsync(user.Id, sessionId);

        JwtTokenResponse response = new()
        {
            RefreshToken = rft.Token,
            ExpiresIn = DateTime.UtcNow.AddMinutes(addMinutes),
            IssuedAt = rft.CreatedAt,
        };

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(Configurations.GetConfiguratinValue<string>("IdentityParameters", "SecurityKey")));
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
            audience: Configurations.GetConfiguratinValue<string>("IdentityParameters", "Audience"),
            issuer: Configurations.GetConfiguratinValue<string>("IdentityParameters", "Issuer"),
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
            Expires = DateTime.UtcNow.AddMinutes(Configurations.GetConfiguratinValue<int>("IdentityParameters", "RefreshTokenExpirationInMinutes")),
            CreatedByIp = _userDeviceInfoService.GetClientIp(),
            SessionId = sessionId,
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
                token.IsRevoked = true;
                token.RevokedByIp = _userDeviceInfoService.GetClientIp();

                _tokenRepository.Update(token);
            }
        }
        await _tokenRepository.SaveChangesAsync();
    }
}