using BlogApp.Application.DTOs.Responses;
using MediatR;

namespace BlogApp.Application.Commands.UserCommand
{
    public record UpdateUserProfileCommand(
      int UserId,
      string? Username = null,
      string? ProfileImageUrl = null
  ) : IRequest<UserResponseDto>;
}
