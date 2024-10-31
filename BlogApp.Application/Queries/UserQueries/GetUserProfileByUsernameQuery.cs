using BlogApp.Application.DTOs.Responses;
using MediatR;

namespace BlogApp.Application.Queries.UserQueries
{
    public record GetUserProfileByUsernameQuery(string Username) : IRequest<UserResponseDto?>;
}
