using System.ComponentModel.DataAnnotations;
using chat_app.Api.Features.Communities;

namespace chat_app.Api.Features.Users
{
    public readonly record struct UserId(Guid Value)
    {
        public static UserId NewId => new(Guid.NewGuid());
        public static UserId Empty => new(Guid.Empty);
    }

    public class User
    {
        [Required, Key]
        public UserId Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Invalid Length: Needs to be at least 2 characters")]
        [MaxLength(10, ErrorMessage = "Invalid Length: Cannot exceed 10 characters")]
        public string Username { get; init; }

        [Required]
        public string PasswordHash { get; init; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Updated { get; set; }

        public virtual ICollection<Community> Communities { get; set; } = new HashSet<Community>();
    }
}
