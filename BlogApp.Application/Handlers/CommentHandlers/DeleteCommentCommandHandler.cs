using BlogApp.Application.Commands.CommentCommands;
using BlogApp.Application.Services;
using MediatR;

namespace BlogApp.Application.Handlers.CommentHandlers
{
    public class DeleteCommentCommandHandler(ICommentService commentService) : IRequestHandler<DeleteCommentCommand, bool>
    {
        public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            return await commentService.DeleteCommentByIdAsync(request.CommentId);
        }
    }
}
