using BlogApp.Application.DTOs.Responses;
using MediatR;

namespace BlogApp.Application.Queries.PostQueries
{
    public record GetUserPostsQuery(int UserId) : IRequest<IEnumerable<PostResponseDto>>;

}
