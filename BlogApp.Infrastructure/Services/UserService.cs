using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Domain.Repositories;
using BlogApp.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using static System.Net.WebRequestMethods;

namespace BlogApp.Infrastructure.Services
{
    public class UserService(
        IUserRepository userRepository
) : IUserService
    {
        public async Task<UserResponseDto> UpdateUserProfileAsync(int userId, string? username, string? profileImageUrl)
        {
            var user = await userRepository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

            if (username != null)
            {
                var existingUser = await userRepository.GetByUsernameAsync(username);
                if (existingUser != null && existingUser.UserId != userId)
                {
                    throw new ValidationException("Username is already taken.");
                }
                user.Username = username;
            }

            if(profileImageUrl != null)
            {
                user.ProfileImageUrl = profileImageUrl;
            } else
            {
                user.ProfileImageUrl = "https://res.cloudinary.com/dcj1bb8vk/image/upload/v1728852537/blogapp/default-user_uo85fb.jpg";
            }

            user = await userRepository.UpdateAsync(user);

            return new UserResponseDto
            {
                Id = user.UserId,
                Username = user.Username,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                ProfileImageUrl = user.ProfileImageUrl,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
