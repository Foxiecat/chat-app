using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using chat_app.Api.Features.Auth.Interfaces;
using chat_app.Api.Features.Members;
using Microsoft.IdentityModel.Tokens;

namespace chat_app.Api.Features.Auth;

public class TokenService(IConfiguration config) : ITokenService
{
    public string GenerateToken(Member member)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, member.Id.ToString()),
            new(ClaimTypes.Name, member.Username),
        ];

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                config["Jwt:key"] ?? throw new NoNullAllowedException("Jwt Key cannot be null!!!")
            )
        );

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken token = new(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(
                double.Parse(
                    config["Jwt:ExpiryHours"]
                        ?? throw new NoNullAllowedException("Jwt:ExpiryHours is missing!!!")
                )
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
