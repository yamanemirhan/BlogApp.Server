using BlogApp.Application.Commands.CommentCommands;
using BlogApp.Application.Services;
using MediatR;

namespace BlogApp.Application.Handlers.CommentHandlers
{
    public class UpdateCommentCommandHandler(ICommentService commentService) : IRequestHandler<UpdateCommentCommand, Comment>
    {
        public async Task<Comment> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            return await commentService.UpdateCommentAsync(request.CommentId, request.Content);
        }
    }
}
