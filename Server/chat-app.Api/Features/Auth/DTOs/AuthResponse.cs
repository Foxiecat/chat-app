namespace chat_app.Api.Features.Auth.DTOs;

public record AuthResponse(string Token, Guid MemberId, string Username);
