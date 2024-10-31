using BlogApp.Application.DTOs.Requests;
using BlogApp.Application.DTOs.Responses;
using MediatR;

namespace BlogApp.Application.Commands.AuthCommands
{
    public record LoginCommand(LoginRequestDto LoginRequest) : IRequest<LoginResponseDto>;
}
