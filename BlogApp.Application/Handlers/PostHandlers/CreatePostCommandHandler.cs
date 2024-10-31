using BlogApp.Application.Commands.PostCommands;
using BlogApp.Application.Interfaces;
using MediatR;

namespace BlogApp.Application.Handlers.PostHandlers
{
    public class CreatePostCommandHandler(IPostService postService) : IRequestHandler<CreatePostCommand, Post>
    {
        public async Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            return await postService.CreatePostAsync(request.CreatePostRequestDto, request.UserId);
        }
    }
}
