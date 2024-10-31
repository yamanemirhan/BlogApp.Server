using MediatR;

namespace BlogApp.Application.Commands.CommentCommands
{
    public record DeleteCommentCommand(int CommentId) : IRequest<bool>;
}
