using chat_app.Api.Features.Channels;
using chat_app.Api.Features.Members;

namespace chat_app.Api.Features.Messages;

public class Message
{
    public required Guid Id { get; init; }
    public required Guid ChannelId { get; init; }
    public required Guid SenderId { get; init; }
    public required string Content { get; set; }
    public required bool IsDeleted { get; set; }
    public required DateTime Created { get; init; }
    public DateTime? Updated { get; set; }

    public virtual Member Sender { get; set; } = null!;
    public virtual Channel Channel { get; set; } = null!;
}
