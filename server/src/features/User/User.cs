namespace src.features.User;

public readonly record struct UserId(Guid Value)
{
    public static UserId NewId() => new(Guid.NewGuid());
    public static UserId Empty => new(Guid.Empty);
}

public class User
{
    public UserId Id { get; set; }
    public string? Username { get; set; }
    public int? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}