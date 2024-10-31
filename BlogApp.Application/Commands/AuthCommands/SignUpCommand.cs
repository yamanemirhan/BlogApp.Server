using BlogApp.Application.DTOs.Requests;
using MediatR;

namespace BlogApp.Application.Commands.AuthCommands
{
    public record SignUpCommand(SignUpRequestDto SignUpRequest) : IRequest<bool>;
}
