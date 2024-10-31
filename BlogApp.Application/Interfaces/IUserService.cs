using BlogApp.Application.DTOs.Responses;

namespace BlogApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> UpdateUserProfileAsync(
            int userId,
            string? username,
            string? profileImageUrl
         );
    }
}
