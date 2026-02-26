using src.Services;

namespace src.Middleware;

public class JwtMiddleware(
    ITokenService tokenService,
    ILogger<JwtMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string? token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (token is not null)
        {
            string? userId = tokenService.ValidateAccessToken(token);
            logger.LogInformation("User: {UserId}", userId);

            context.Items["UserId"] = userId;
        }
        else
        {
            logger.LogInformation("No token found");
        }
        await next(context);
    }
}

