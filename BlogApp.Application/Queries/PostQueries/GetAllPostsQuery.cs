using BlogApp.Application.DTOs.Responses;
using MediatR;

namespace BlogApp.Application.Queries.PostQueries
{
    public record GetAllPostsQuery(string? Search,
        DateTime? FromDate,
        DateTime? ToDate,
        List<string>? CategoryNames,
        List<string>? TagNames,
        string? SortBy,
        bool IsDescending, int PageNumber, int PageSize) : IRequest<PaginatedResponseDto<PostResponseDto>>;

}
