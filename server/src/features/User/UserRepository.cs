using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using src.Database;
using src.features.User.Interfaces;

namespace src.features.User;

public class UserRepository(UsersDbContext dbContext) : IUserRepository
{
    public async Task<User> AddAsync(User entity)
    {
        dbContext.Users.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(user => user.Id.Value == id);
    }

    public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
    {
        return await dbContext.Users
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<User> UpdateAsync(User entity)
    {
        dbContext.Users.Update(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }
}