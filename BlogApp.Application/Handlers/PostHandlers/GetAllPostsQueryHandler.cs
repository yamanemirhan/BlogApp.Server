using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Queries.PostQueries;
using BlogApp.Domain.DTOs.Requests;
using MediatR;

namespace BlogApp.Application.Handlers.PostHandlers
{
    public class GetAllPostsQueryHandler(IPostService postService) : IRequestHandler<GetAllPostsQuery, PaginatedResponseDto<PostResponseDto>>
    {
        public  Task<PaginatedResponseDto<PostResponseDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var queryDto = new GetAllPostsQueryDto(
            request.Search,
            request.FromDate,
            request.ToDate,
            request.CategoryNames,
            request.TagNames,
            request.SortBy,
            request.IsDescending,
            request.PageNumber,
            request.PageSize
            );

            return postService.GetAllPostsAsync(queryDto);
        }
    }
}
