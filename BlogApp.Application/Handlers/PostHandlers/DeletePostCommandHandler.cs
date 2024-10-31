using BlogApp.Application.Commands.PostCommands;
using BlogApp.Application.Interfaces;
using MediatR;

namespace BlogApp.Application.Handlers.PostHandlers
{
    public class DeletePostCommandHandler(IPostService postService) : IRequestHandler<DeletePostCommand, bool>
    {
        public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            return await postService.DeletePostByIdAsync(request.PostId);
        }
    }
}

