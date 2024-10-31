using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Queries.UserQueries;
using BlogApp.Domain.Repositories;
using MediatR;

namespace BlogApp.Application.Handlers.UserHandlers
{
    public class GetUserProfileByUsernameHandler(IUserRepository userRepository) : IRequestHandler<GetUserProfileByUsernameQuery, UserResponseDto?>
    {
        public async Task<UserResponseDto?> Handle(GetUserProfileByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByUsernameAsync(request.Username);

            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                Id = user.UserId,
                Username = user.Username,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                ProfileImageUrl = user.ProfileImageUrl,
                CreatedAt = user.CreatedAt,
            };
        }
    }
}
