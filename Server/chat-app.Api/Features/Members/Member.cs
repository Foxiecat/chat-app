using chat_app.Api.Features.Communities;

namespace chat_app.Api.Features.Members
{
    public class Member
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public virtual ICollection<Community> Communities { get; set; } = new HashSet<Community>();
    }
}
