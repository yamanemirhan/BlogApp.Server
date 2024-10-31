using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Queries.UserQueries;
using BlogApp.Domain.Repositories;
using MediatR;

namespace BlogApp.Application.Handlers.UserHandlers
{
    public class GetCurrentUserProfileHandler(IUserRepository userRepository) : IRequestHandler<GetCurrentUserProfileQuery, UserResponseDto>
    {
        public async Task<UserResponseDto?> Handle(GetCurrentUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId);

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
