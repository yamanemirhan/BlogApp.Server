using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Queries.PostQueries;
using BlogApp.Domain.Repositories;
using MediatR;

namespace BlogApp.Application.Handlers.PostHandlers
{
    public class GetPostByIdQueryHandler(IPostService postService) : IRequestHandler<GetPostByIdQuery, PostResponseDto>
    {
        public async Task<PostResponseDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            return await postService.GetPostByIdAsync(request.Id);
        }
    }
}
