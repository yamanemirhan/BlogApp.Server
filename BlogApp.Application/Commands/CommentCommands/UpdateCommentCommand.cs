using MediatR;

namespace BlogApp.Application.Commands.CommentCommands
{
    public record UpdateCommentCommand(int CommentId, string Content) : IRequest<Comment>;

}
