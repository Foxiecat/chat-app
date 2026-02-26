namespace src.features.User.DTOs;

public record UserRequest(string? Username, int? PhoneNumber, string? Email, string? Password);

