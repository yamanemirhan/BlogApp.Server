using BlogApp.Domain.Entities;

namespace BlogApp.Domain.Repositories
{
    public interface IImageRepository
    {
        Task<PostImage> CreateAsync(PostImage image);
        Task<IEnumerable<PostImage>> GetByPostIdAsync(int postId);
    }
}
