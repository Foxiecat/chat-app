using src.features.User;

namespace src.Services;

public interface ITokenService
{
    string CreateToken(User user);
    string CreateRefreshToken();
    string? ValidateAccessToken(string accessToken);
    string? ValidateRefreshToken(string refreshToken, User user);
}

