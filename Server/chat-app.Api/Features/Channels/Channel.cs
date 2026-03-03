using System.ComponentModel.DataAnnotations;
using chat_app.Api.Features.Communities;
using chat_app.Api.Features.Users;

namespace chat_app.Api.Features.Channels;

public readonly record struct ChannelId(Guid Value)
{
    public static ChannelId NewId => new(Guid.NewGuid());
    public static ChannelId Empty => new(Guid.Empty);
}

public class Channel
{
    [Required, Key]
    public ChannelId Id { get; set; }

    [Required]
    [MaxLength(30, ErrorMessage = "Invalid Length: Name cannot exceed 30 characters!")]
    public string Name { get; set; }

    public virtual required Community Community { get; set; }
    public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
}
