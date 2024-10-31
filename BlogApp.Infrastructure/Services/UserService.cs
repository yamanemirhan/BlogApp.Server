using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Domain.Repositories;
using System.ComponentModel.DataAnnotations;

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

            Console.WriteLine("PROFILE IMAGE URL: " + profileImageUrl);
            Console.WriteLine("USER PROFILE IMAGE URL: " + user.ProfileImageUrl);


            if (profileImageUrl != null)
            {
                user.ProfileImageUrl = string.IsNullOrEmpty(profileImageUrl)
                    ? "https://res.cloudinary.com/dcj1bb8vk/image/upload/v1728852537/blogapp/default-user_uo85fb.jpg"
                    : profileImageUrl;
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
