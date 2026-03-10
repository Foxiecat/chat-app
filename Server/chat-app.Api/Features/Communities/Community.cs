using chat_app.Api.Features.Channels;
using chat_app.Api.Features.CommunityMembers;
using chat_app.Api.Features.Members;

namespace chat_app.Api.Features.Communities;

public class Community
{
    public Guid Id { get; set; }
    public required Guid OwnerId { get; set; }
    public required string Name { get; set; }
    public required DateTime Created { get; set; }
    public DateTime? Updated { get; set; }

    public virtual Member Owner { get; set; } = null!;
    public virtual ICollection<Channel> Channels { get; set; } = new HashSet<Channel>();
    public virtual ICollection<CommunityMember> CommunityMemberships { get; set; } =
        new HashSet<CommunityMember>();
}
