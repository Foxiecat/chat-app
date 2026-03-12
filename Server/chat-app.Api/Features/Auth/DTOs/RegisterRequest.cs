using System.ComponentModel.DataAnnotations;

namespace chat_app.Api.Features.Auth.DTOs;

public record RegisterRequest(
    [Required, MinLength(3), MaxLength(10)] string Username,
    [Required, MinLength(8)] string Password
);
