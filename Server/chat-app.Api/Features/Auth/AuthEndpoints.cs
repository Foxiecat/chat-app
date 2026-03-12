using chat_app.Api.Database;
using chat_app.Api.Features.Auth.DTOs;
using chat_app.Api.Features.Auth.Endpoints;
using chat_app.Api.Features.Auth.Interfaces;
using chat_app.Api.Features.Members;
using Microsoft.EntityFrameworkCore;

namespace chat_app.Api.Features.Auth;

public class AuthEndpoints(
    RegisterService registerService,
    LoginService loginService)
{
    public void MapAuthEndpoints(WebApplication app)
    {
        var authGroup = app.MapGroup("/auth");

        authGroup.MapPost("/register", registerService.HandleAsync).WithName("Register").AllowAnonymous();
        authGroup.MapPost("/login", loginService.HandleAsync).WithName("Login").AllowAnonymous();
    }

    private static async Task<IResult> RegisterAsync(
        ChatDbContext dbContext,
        ITokenService tokenService,
        RegisterRequest request
    )
    {
        if (
            string.IsNullOrWhiteSpace(request.Username)
            || string.IsNullOrWhiteSpace(request.Password)
        )
        {
            return Results.BadRequest("Username and password are required.");
        }

        var exists = await dbContext.Member.AnyAsync(member => member.Username == request.Username);
        if (exists)
            return Results.Conflict("Username already taken!");

        Member member = new()
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Status = "offline",
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password),
            Created = DateTime.UtcNow,
        };

        dbContext.Member.Add(member);
        await dbContext.SaveChangesAsync();

        AuthResponse response = new(tokenService.GenerateToken(member), member.Id, member.Username);
        return Results.Ok(response);
    }
}
