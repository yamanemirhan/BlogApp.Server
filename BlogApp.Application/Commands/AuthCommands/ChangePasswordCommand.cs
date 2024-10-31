using BlogApp.Application.DTOs.Responses;
using MediatR;

namespace BlogApp.Application.Commands.AuthCommands
{
    public record ChangePasswordCommand(
        int UserId,
        string OldPassword,
        string NewPassword
    ) : IRequest<ChangePasswordResponseDto>;
}
