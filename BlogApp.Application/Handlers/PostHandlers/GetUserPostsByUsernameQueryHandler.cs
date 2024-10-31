using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Queries.PostQueries;
using MediatR;

namespace BlogApp.Application.Handlers.PostHandlers
{
    public class GetUserPostsByUsernameQueryHandler(IPostService postService) : IRequestHandler<GetUserPostsByUsernameQuery, IEnumerable<PostResponseDto>>
    {
        public async Task<IEnumerable<PostResponseDto>> Handle(GetUserPostsByUsernameQuery request, CancellationToken cancellationToken)
        {
            return await postService.GetPostsByUsernameAsync(request.Username);
        }
    }
}
