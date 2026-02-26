using src.features.shared.Interfaces;
using src.features.User.DTOs;
using src.features.User.Interfaces;
using src.Services;
using static BCrypt.Net.BCrypt;

namespace src.features.User.endpoints;

public class LoginUserEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder app)
    {
        app.MapPost("user/auth/login", HandleAsync)
            .WithName("Login")
            .WithTags("Auth")
            .AllowAnonymous();
    }

    private static async Task<IResult> HandleAsync(
        IUserRepository userRepository,
        ITokenService tokenService,
        LoginRequest request,
        CancellationToken ct)
    {
        IEnumerable<User> users = await userRepository.FindAsync(user => user.Username == request.Username);
        User? user = users.FirstOrDefault();

        if (user is null)
            return Results.NotFound(ct);

        if (!EnhancedVerify(request.Password, user!.PasswordHash))
            return Results.Unauthorized();

        string accessToken = tokenService.CreateToken(user);
        string refreshToken = tokenService.CreateRefreshToken();
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await userRepository.UpdateAsync(user);

        AuthResponse response = new(accessToken, refreshToken, user);
        return Results.Ok(response);
    }
}
