using BlogApp.Application.DTOs.Responses;

namespace BlogApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ChangePasswordResponseDto> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }
}
