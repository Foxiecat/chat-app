namespace src.features.User.DTOs;

public record AuthResponse(string AccessToken, string RefreshToken, User User);
