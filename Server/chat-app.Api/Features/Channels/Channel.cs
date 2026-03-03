using chat_app.Api.Features.Communities;
using chat_app.Api.Features.Users;

namespace chat_app.Api.Features.Channels;

public class Channel
{
    public required Guid Id { get; init; }

    public Guid CommunityId { get; init; }

    public required string Name { get; set; }

    public virtual required Community Community { get; set; }
    public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
}
