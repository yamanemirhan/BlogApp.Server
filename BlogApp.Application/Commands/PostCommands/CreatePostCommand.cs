using BlogApp.Application.DTOs.Requests;
using MediatR;

namespace BlogApp.Application.Commands.PostCommands
{
    public record CreatePostCommand(CreatePostRequestDto CreatePostRequestDto, int UserId) : IRequest<Post>;
}
