using chat_app.Api.Features.Auth.Endpoints;

namespace chat_app.Api.Features.Auth;

public class AuthEndpoints(
    RegisterService registerService,
    LoginService loginService)
{
    // TODO: Map the Endpoints in program.cs
    public void MapAuthEndpoints(WebApplication app)
    {
        var authGroup = app.MapGroup("/auth");

        authGroup.MapPost("/register", registerService.HandleAsync).WithName("Register").AllowAnonymous();
        authGroup.MapPost("/login", loginService.HandleAsync).WithName("Login").AllowAnonymous();
    }
}
