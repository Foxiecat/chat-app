using src.features.shared.Interfaces;
using src.features.User.DTOs;
using src.features.User.Interfaces;
using static BCrypt.Net.BCrypt;

namespace src.features.User.endpoints;

public class RegisterUserEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder app)
    {
        app.MapPost("user/auth/register", HandleAsync)
            .WithName("Register")
            .WithTags("Auth")
            .AllowAnonymous();
    }

    private static async Task<IResult> HandleAsync(
        IUserRepository userRepository,
        UserRequest request,
        CancellationToken ct)
    {
        IEnumerable<User> existingUsers = await userRepository.FindAsync(u => u.Username == request.Username || u.Email == request.Email);
        if (existingUsers.Any())
            return Results.BadRequest("User already exists");

        User user = new()
        {
            Id = UserId.NewId(),
            Username = request.Username,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            PasswordHash = EnhancedHashPassword(request.Password)
        };

        await userRepository.AddAsync(user);

        // TODO: Add mapping to user entity to response dto
        return Results.Ok(user);
    }
}