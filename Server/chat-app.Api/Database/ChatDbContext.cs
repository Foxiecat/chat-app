using chat_app.Api.Features.Channels;
using chat_app.Api.Features.Communities;
using chat_app.Api.Features.Members;
using Microsoft.EntityFrameworkCore;

namespace chat_app.Api.Database;

public class ChatDbContext(DbContextOptions<ChatDbContext> options) : DbContext(options)
{
    public DbSet<Member> Member { get; set; }
    public DbSet<Community> Community { get; set; }
    public DbSet<Channel> Channel { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Member>(entity =>
        {
            entity.ToTable(name: "Member");
            entity.HasMany(user => user.Communities).WithMany(community => community.Members);
        });

        builder.Entity<Community>(entity =>
        {
            entity.ToTable(name: "Community");
            entity
                .HasMany(community => community.Channels)
                .WithOne(channel => channel.Community)
                .HasForeignKey(channel => channel.CommunityId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Channel>(entity =>
        {
            entity.ToTable("Channel");
            entity
                .HasOne(channel => channel.Community)
                .WithMany(community => community.Channels)
                .HasForeignKey(channel => channel.CommunityId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
