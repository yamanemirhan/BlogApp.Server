using BlogApp.Application.Commands.UserCommand;
using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using MediatR;

namespace BlogApp.Application.Handlers.UserHandlers
{
    public class UpdateUserProfileCommandHandler(IUserService userService) : IRequestHandler<UpdateUserProfileCommand, UserResponseDto>
    {

        public async Task<UserResponseDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            return await userService.UpdateUserProfileAsync(request.UserId, request.Username, request.ProfileImageUrl);
        }
    }
}
