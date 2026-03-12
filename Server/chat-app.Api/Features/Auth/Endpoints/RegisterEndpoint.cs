using chat_app.Api.Database;
using chat_app.Api.Features.Auth.DTOs;
using chat_app.Api.Features.Auth.Interfaces;
using chat_app.Api.Features.Members;
using chat_app.Api.Features.Shared;
using Microsoft.EntityFrameworkCore;

namespace chat_app.Api.Features.Auth.Endpoints;

public class RegisterService(IHttpContextAccessor accessor) : BaseEndpoint<RegisterRequest, AuthResponse>(accessor)
{
    protected override (bool IsValid, IDictionary<string, string[]> Errors) ValidateRequest(
        RegisterRequest request
    )
    {
        Dictionary<string, string[]> errors = [];
        if (string.IsNullOrWhiteSpace(request.Username))
            errors["Username"] = ["Username is required!"];
        if (string.IsNullOrWhiteSpace(request.Password))
            errors["Password"] = ["Password is required!"];
        return (errors.Count == 0, errors);
    }

    public async Task<IResult> HandleAsync(
        ChatDbContext dbContext,
        ITokenService tokenService,
        RegisterRequest request,
        CancellationToken ct)
    {
        return await ExecuteAsync(
            request,
            action: async cancellationToken =>
            {
                var exists = await dbContext.Member.AnyAsync(member => member.Username == request.Username, cancellationToken: cancellationToken);
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
                await dbContext.SaveChangesAsync(cancellationToken);

                AuthResponse response = new(tokenService.GenerateToken(member), member.Id, member.Username);
                return Ok(response);
            }, ct: ct);
    }
}