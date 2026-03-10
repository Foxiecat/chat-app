using chat_app.Api.Features.CommunityMembers;

namespace chat_app.Api.Features.Members;

public class Member
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Status { get; set; } = "offline";
    public required string PasswordHash { get; set; }
    public required DateTime Created { get; set; }
    public DateTime? Updated { get; set; }

    public virtual ICollection<CommunityMember> CommunityMemberships { get; set; } =
        new HashSet<CommunityMember>();
}
