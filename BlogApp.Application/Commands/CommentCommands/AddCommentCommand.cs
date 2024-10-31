using BlogApp.Application.DTOs.Requests;
using MediatR;

namespace BlogApp.Application.Commands.CommentCommands
{
    public record AddCommentCommand(CreateCommentRequestDto createCommentRequestDto, int UserId) : IRequest<int>;

}
