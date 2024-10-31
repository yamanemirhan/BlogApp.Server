using BlogApp.Domain.Repositories;
using BlogApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Infrastructure.Repositories
{
    public class TagRepository(AppDbContext dbContext) : GenericRepository<Tag>(dbContext), ITagRepository
    {
        public async Task<List<Tag>> GetTagsByIdsAsync(IEnumerable<int> tagIds)
        {
            return await dbContext.Tags.Where(tag => tagIds.Contains(tag.TagId)).ToListAsync();
        }
    }
}
