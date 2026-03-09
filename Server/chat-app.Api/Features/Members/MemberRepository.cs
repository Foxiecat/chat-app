using System.Linq.Expressions;
using chat_app.Api.Database;
using chat_app.Api.Features.Members.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace chat_app.Api.Features.Members
{
    public class MemberRepository(ChatDbContext dbContext, ILogger<MemberRepository> logger)
        : IMemberRepository
    {
        public async Task<IResult> AddAsync(Member entity)
        {
            bool checkIfUserExists = dbContext.Member.Any(member => member.Id == entity.Id);
            if (checkIfUserExists)
            {
                logger.LogWarning("Member with ID {MemberId} already exists!", entity.Id);
                return Results.BadRequest($"{entity.Username} already exists!");
            }

            await dbContext.Member.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return Results.Ok(entity);
        }

        public async Task<IResult> GetByIdAsync(Guid id)
        {
            bool checkIfExists = dbContext.Member.Any(member => member.Id == id);
            if (!checkIfExists)
            {
                logger.LogWarning("Member with ID {MemberId} does not exist!", id);
                return Results.BadRequest("Member does not exist!");
            }

            Member member = await dbContext.Member.FirstOrDefaultAsync(member => member.Id == id);
            return Results.Ok(member);
        }

        public async Task<IEnumerable<Member>> GetPagedAsync(int pageIndex, int pageSize)
        {
            return await dbContext
                .Member.OrderBy(member => member.Username)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Member>> FindAsync(Expression<Func<Member, bool>> predicate)
        {
            return await dbContext.Member.Where(predicate).ToListAsync();
        }

        public async Task<IResult> UpdateAsync(Member entity)
        {
            Member existingMember =
                await dbContext.Member.FindAsync(entity.Id)
                ?? throw new InvalidOperationException($"Could not find Member: {entity.Id}");

            existingMember.Username = entity.Username;
            existingMember.PasswordHash = entity.PasswordHash;
            existingMember.Updated = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return Results.Ok(existingMember);
        }
    }
}
