using Microsoft.EntityFrameworkCore;
using src.features.User;

namespace src.Database;

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                .HasConversion(
                    id => id.Value,
                    value => new UserId(value)
                );
        });
    }
}