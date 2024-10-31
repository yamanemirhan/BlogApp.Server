using BlogApp.Application.DTOs.Responses;
using MediatR;

namespace BlogApp.Application.Commands.PostCommands
{
    public record UpdatePostCommand(
        int PostId,
        string Title,
        string Description,
        string Content,
        int CategoryId,
        string CoverImageUrl,
        List<int> TagIds
        //int UserId // To validate ownership of the post
    ) : IRequest<PostResponseDto>;
}
