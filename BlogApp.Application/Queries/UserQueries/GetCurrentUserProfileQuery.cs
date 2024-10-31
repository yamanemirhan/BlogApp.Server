using BlogApp.Application.DTOs.Responses;
using MediatR;

namespace BlogApp.Application.Queries.UserQueries
{
    public record GetCurrentUserProfileQuery(int UserId) : IRequest<UserResponseDto>;

}
