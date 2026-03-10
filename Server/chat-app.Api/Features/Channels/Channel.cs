using chat_app.Api.Features.Communities;

namespace chat_app.Api.Features.Channels;

public class Channel
{
    public required Guid Id { get; init; }
    public Guid CommunityId { get; init; }
    public required string Name { get; set; }
    public int Position { get; set; } = 0;

    public virtual required Community Community { get; set; }
}
