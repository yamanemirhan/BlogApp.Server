using BlogApp.Application.Commands.AuthCommands;
using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Services;
using BlogApp.Domain.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Application.Handlers.AuthHandlers
{
    public class ChangePasswordCommandHandler(IAuthService authService) : IRequestHandler<ChangePasswordCommand, ChangePasswordResponseDto>
    {
        public async Task<ChangePasswordResponseDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            return await authService.ChangePasswordAsync(request.UserId, request.OldPassword, request.NewPassword);
        }
    }
}
