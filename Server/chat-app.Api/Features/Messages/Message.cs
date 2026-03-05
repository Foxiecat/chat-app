namespace chat_app.Api.Features.Messages
{
    public class Message
    {
        public required Guid Id { get; init; }
        public required Guid ChannelId { get; init; }
        public required Guid SenderId { get; init; }
        public required string Content { get; set; }
        public required bool IsDeleted { get; set; }
        public required DateTime Created { get; init; }
        public DateTime? Updated { get; set; }
    }
}
