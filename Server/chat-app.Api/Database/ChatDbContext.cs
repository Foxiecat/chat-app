using chat_app.Api.Features.Channels;
using chat_app.Api.Features.Communities;
using chat_app.Api.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace chat_app.Api.Database;

public class ChatDbContext(DbContextOptions<ChatDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
    public DbSet<Community> Community { get; set; }
    public DbSet<Channel> Channel { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
