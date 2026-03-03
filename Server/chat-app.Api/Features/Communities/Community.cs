using System.ComponentModel.DataAnnotations;
using System.Threading.Channels;
using chat_app.Api.Features.Users;

namespace chat_app.Api.Features.Communities;

public readonly record struct CommunityId(Guid Value)
{
    public static CommunityId NewId => new(Guid.NewGuid());
    public static CommunityId Empty => new(Guid.Empty);
}

public class Community
{
    [Required, Key]
    public CommunityId Id { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "Invalid Length: Name needs to be at least 3 characters")]
    [MaxLength(30, ErrorMessage = "Invalid Length: Name cannot exceed 30 characters!")]
    public string Name { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public DateTime Updated { get; set; }

    public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    public virtual ICollection<Channel> Channels { get; set; } = new HashSet<Channel>();
}
