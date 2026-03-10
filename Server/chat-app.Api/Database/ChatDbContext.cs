using chat_app.Api.Features.Channels;
using chat_app.Api.Features.Communities;
using chat_app.Api.Features.CommunityMembers;
using chat_app.Api.Features.Members;
using chat_app.Api.Features.Messages;
using Microsoft.EntityFrameworkCore;

namespace chat_app.Api.Database;

public class ChatDbContext(DbContextOptions<ChatDbContext> options) : DbContext(options)
{
    public DbSet<Member> Member { get; set; }
    public DbSet<Community> Community { get; set; }
    public DbSet<Channel> Channel { get; set; }
    public DbSet<CommunityMember> CommunityMember { get; set; }
    public DbSet<Message> Message { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable(name: "Member");
            entity
                .HasMany(user => user.CommunityMemberships)
                .WithOne(cm => cm.Member)
                .HasForeignKey(cm => cm.MemberId);
        });

        modelBuilder.Entity<Community>(entity =>
        {
            entity.ToTable(name: "Community");
            entity
                .HasOne(c => c.Owner)
                .WithMany()
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasMany(c => c.CommunityMemberships)
                .WithOne(cm => cm.Community)
                .HasForeignKey(cm => cm.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasMany(c => c.Channels)
                .WithOne(channel => channel.Community)
                .HasForeignKey(channel => channel.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityMember>(entity =>
        {
            entity.ToTable("CommunityMember");
            entity.HasKey(cm => new { cm.MemberId, cm.CommunityId });
        });

        modelBuilder.Entity<Channel>(entity => entity.ToTable("Channel"));

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Message");
            entity
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(m => m.Channel)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(m => new { m.ChannelId, m.Created });
        });
    }
}
