using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Queries.PostQueries;
using BlogApp.Domain.Repositories;
using MediatR;

namespace BlogApp.Application.Handlers.PostHandlers
{
    public class GetUserPostsQueryHandler(IPostService postService) : IRequestHandler<GetUserPostsQuery, IEnumerable<PostResponseDto>>
    {
        public async Task<IEnumerable<PostResponseDto>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            return await postService.GetPostsByUserIdAsync(request.UserId);
        }
    }
}
