using BlogApp.Domain.Entities;

namespace BlogApp.Domain.Repositories
{
    public interface IPostTagRepository : IGenericRepository<PostTag>
    {
        Task RemovePostTags(List<PostTag> postTags);
        Task CreatePostTags(List<PostTag> postTags);

    }
}
