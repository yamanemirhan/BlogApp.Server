using BlogApp.Domain.Entities;
using BlogApp.Domain.Repositories;
using BlogApp.Infrastructure.Data;

namespace BlogApp.Infrastructure.Repositories
{
    public class PostTagRepository(AppDbContext dbContext) : GenericRepository<PostTag>(dbContext), IPostTagRepository
    {
        public async Task RemovePostTags(List<PostTag> postTags)
        {
            dbContext.PostTags.RemoveRange(postTags);
            await SaveChangesAsync();
        }

        public async Task CreatePostTags(List<PostTag> postTags)
        {
            await dbContext.PostTags.AddRangeAsync(postTags);
            await SaveChangesAsync();
        }
    }
}
