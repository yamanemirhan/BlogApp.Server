namespace BlogApp.Domain.DTOs.Requests
{
    public record GetAllPostsQueryDto(
        string? Search,
        DateTime? FromDate,
        DateTime? ToDate,
        List<string>? CategoryNames,
        List<string>? TagNames, 
        string? SortBy, 
        bool IsDescending,
        int PageNumber,
        int PageSize
    );
}
