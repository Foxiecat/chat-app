using chat_app.Api.Database;
using chat_app.Api.Features.Auth.DTOs;
using chat_app.Api.Features.Auth.Interfaces;
using chat_app.Api.Features.Shared;
using Microsoft.EntityFrameworkCore;

namespace chat_app.Api.Features.Auth.Endpoints;

public class LoginService(IHttpContextAccessor accessor)
    : BaseEndpoint<LoginRequest, AuthResponse>(accessor)
{
    protected override (bool IsValid, IDictionary<string, string[]> Errors) ValidateRequest(
        LoginRequest request
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
        LoginRequest request,
        CancellationToken ct
    )
    {
        return await ExecuteAsync(
            request,
            action: async cancellationToken =>
            {
                var member = await dbContext.Member.FirstOrDefaultAsync(member =>
                    member.Username == request.Username
                );

                if (
                    member is null
                    || !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, member.PasswordHash)
                )
                {
                    return Results.Unauthorized();
                }

                AuthResponse response = new(
                    tokenService.GenerateToken(member),
                    member.Id,
                    member.Username
                );

                return Ok(response);
            },
            ct: ct
        );
    }
}
