namespace BlogApp.Application.DTOs.Requests
{
    public record UpdatePostRequestDto(
       string Title,
       string Description,
       string Content,
       int CategoryId,
       string CoverImageUrl,
       List<int> TagIds
   );
}
