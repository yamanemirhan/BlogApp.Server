using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Services;
using BlogApp.Domain.Repositories;

namespace BlogApp.Infrastructure.Services
{
    public class AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher) : IAuthService
    {
        public async Task<ChangePasswordResponseDto> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                Console.WriteLine("User not found!");
                return new ChangePasswordResponseDto(false, "Something went wrong!");
            }

            if (!passwordHasher.VerifyPassword(user.PasswordHash, oldPassword))
            {
                return new ChangePasswordResponseDto(false, "Old password is incorrect.");
            }

            if (newPassword.Length < 7)
            {
                return new ChangePasswordResponseDto(false, "New password must be at least 7 characters long.");
            }

            user.PasswordHash = passwordHasher.HashPassword(newPassword);
            await userRepository.UpdateAsync(user);

            return new ChangePasswordResponseDto(true, "Password updated successfully.");
        }
    }
}
