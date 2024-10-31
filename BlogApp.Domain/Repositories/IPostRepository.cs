using BlogApp.Domain.DTOs.Requests;
using BlogApp.Domain.DTOs.Responses;

namespace BlogApp.Domain.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
       Task<Post> CreateAsync(Post post);
       Task<Post> GetPostByIdAsync(int postId);
       Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
        Task<IEnumerable<Post>> GetPostsByUsernameAsync(string username);
       Task<Post> UpdateAsync(Post post);

        //Task<List<Post>> GetAllPostsAsync(GetAllPostsQueryDto query);
        Task<PaginatedResult<Post>> GetAllPostsAsync(GetAllPostsQueryDto query);
    }
}
