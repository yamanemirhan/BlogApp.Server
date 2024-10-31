using BlogApp.Application.Commands.CommentCommands;
using BlogApp.Application.Services;
using MediatR;

namespace BlogApp.Application.Handlers.CommentHandlers
{
    public class AddCommentCommandHandler(ICommentService commentService) : IRequestHandler<AddCommentCommand, int>
    {
        public async Task<int> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var commentId = await commentService.AddCommentAsync(request.createCommentRequestDto, request.UserId);
            return commentId;
        }
    }
}
