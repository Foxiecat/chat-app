using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using src.features.User;
using src.Utilities;

namespace src.Services;

public class TokenService(IOptions<JwtOptions> options) : ITokenService
{
    public string CreateToken(User user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
        ];

        SymmetricSecurityKey key = new(
            Encoding.UTF8.GetBytes(
                options.Value.Key ?? throw new NoNullAllowedException("JWT Key cannot be null!")
            )
        );
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = options.Value.Issuer,
            Audience = options.Value.Audience,
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = credentials,
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string CreateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string? ValidateAccessToken(string accessToken)
    {
        try
        {
            JwtSecurityTokenHandler tokenHandler = new();
            SymmetricSecurityKey key = new(
                Encoding.UTF8.GetBytes(options.Value.Key ?? throw new InvalidOperationException())
            );

            tokenHandler.ValidateToken(
                accessToken,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = key,
                    ValidIssuer = options.Value.Issuer,
                    ValidAudience = options.Value.Audience,
                    ClockSkew = TimeSpan.Zero,
                },
                out SecurityToken validatedToken
            );

            JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;

            string userId = jwtToken
                .Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier)
                .Value;
            return userId;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public string? ValidateRefreshToken(string refreshToken, User user)
    {
        if (user.RefreshToken != refreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            return null;
        return user.Id.ToString();
    }
}
