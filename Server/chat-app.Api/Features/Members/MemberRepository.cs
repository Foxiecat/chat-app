using System.Linq.Expressions;
using chat_app.Api.Database;
using chat_app.Api.Features.Members.Interfaces;

namespace chat_app.Api.Features.Members
{
    public class MemberRepository(ChatDbContext dbContext, ILogger<MemberRepository> logger)
        : IMemberRepository
    {
        public async Task<Member?> AddAsync(Member entity)
        {
            bool checkIfUserExists = dbContext.Member.Any(member => member.Id == entity.Id);
            if (checkIfUserExists)
            {
                logger.LogWarning("Member with ID {MemberId} already exists!", entity.Id);
                return null;
            }

            await dbContext.Member.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Member> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Member>> GetPagedAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Member>> FindAsync(Expression<Func<Member, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Member> UpdateAsync(Member entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
