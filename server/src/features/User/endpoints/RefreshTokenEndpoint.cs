using src.features.shared.Interfaces;
using src.features.User.DTOs;
using src.features.User.Interfaces;
using src.Services;

namespace src.features.User.endpoints;

public class RefreshTokenEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder app)
    {
        app.MapPost("user/auth/refresh", HandleAsync)
            .WithName("RefreshToken")
            .WithTags("Auth")
            .AllowAnonymous();
    }

    private static async Task<IResult> HandleAsync(
        IUserRepository userRepository,
        ITokenService tokenService,
        RefreshRequest request, 
        CancellationToken ct)
    {
        IEnumerable<User> users = await userRepository.FindAsync(u => u.RefreshToken == request.RefreshToken);
        User? user = users.FirstOrDefault();

        if (user is null || user.RefreshTokenExpiry < DateTime.UtcNow)
            return Results.Unauthorized();

        string? userId = tokenService.ValidateRefreshToken(request.RefreshToken, user);
        if (userId is null)
            return Results.Unauthorized();

        string accessToken = tokenService.CreateToken(user);
        string newRefreshToken = tokenService.CreateRefreshToken();
        
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await userRepository.UpdateAsync(user);

        AuthResponse response = new(accessToken, newRefreshToken, user);
        return Results.Ok(response);
    }
}
