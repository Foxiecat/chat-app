using chat_app.Api.Features.Channels;
using chat_app.Api.Features.Users;

namespace chat_app.Api.Features.Communities;

public class Community
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateTime Created { get; set; }
    public DateTime? Updated { get; set; }

    public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    public virtual ICollection<Channel> Channels { get; set; } = new HashSet<Channel>();
}
