using chat_app.Api.Features.Channels;
using chat_app.Api.Features.Members;

namespace chat_app.Api.Features.Communities;

public class Community
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateTime Created { get; set; }
    public DateTime? Updated { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new HashSet<Member>();
    public virtual ICollection<Channel> Channels { get; set; } = new HashSet<Channel>();
}
