using chat_app.Api.Features.Communities;
using chat_app.Api.Features.Members;

namespace chat_app.Api.Features.CommunityMembers;

public class CommunityMember
{
    public Guid MemberId { get; init; }
    public Guid CommunityId { get; init; }
    public string? Nickname { get; set; }
    public DateTime JoinedAt { get; init; }

    public virtual Member Member { get; set; } = null!;
    public virtual Community Community { get; set; } = null!;
}
