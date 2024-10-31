using BlogApp.Application.Commands.PostCommands;
using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.DTOs.Requests;
using BlogApp.Domain.DTOs.Requests;

namespace BlogApp.Application.Interfaces
{
    public interface IPostService
    {
        Task<Post> CreatePostAsync(CreatePostRequestDto createPostDto, int userId);
        Task<PostResponseDto> GetPostByIdAsync(int postId);
        Task<IEnumerable<PostResponseDto>> GetPostsByUserIdAsync(int userId);
        Task<IEnumerable<PostResponseDto>> GetPostsByUsernameAsync(string username);
        Task<PostResponseDto> UpdatePostAsync(UpdatePostCommand command);
        Task<bool> DeletePostByIdAsync(int postId);
        //Task<List<PostResponseDto>> GetAllPostsAsync(GetAllPostsQueryDto queryDto);
        Task<PaginatedResponseDto<PostResponseDto>> GetAllPostsAsync(GetAllPostsQueryDto queryDto);
    }
}