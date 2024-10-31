using BlogApp.Application.Commands.PostCommands;
using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using MediatR;

namespace BlogApp.Application.Handlers.PostHandlers
{
    public class UpdatePostCommandHandler(IPostService postService) : IRequestHandler<UpdatePostCommand, PostResponseDto>
    {
        public async Task<PostResponseDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            return await postService.UpdatePostAsync(request);
        }
    }
}
