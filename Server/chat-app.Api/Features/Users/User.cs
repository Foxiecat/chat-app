using chat_app.Api.Features.Communities;

namespace chat_app.Api.Features.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Username { get; init; }
        public required string PasswordHash { get; init; }
        public required DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public virtual ICollection<Community> Communities { get; set; } = new HashSet<Community>();
    }
}
