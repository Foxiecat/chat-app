using Microsoft.AspNetCore.Mvc;

namespace chat_app.Api.Features.Messages;

[Route("/message")]
public static class MessageEndpoints
{
    public static void MapMessageEndpoints(this WebApplication app)
    {
        var messageGroup = app.MapGroup("/message");
    }
}
