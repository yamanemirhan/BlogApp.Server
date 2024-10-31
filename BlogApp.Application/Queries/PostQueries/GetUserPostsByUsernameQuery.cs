using BlogApp.Application.DTOs.Responses;
using MediatR;

namespace BlogApp.Application.Queries.PostQueries
{
    public record GetUserPostsByUsernameQuery(string Username) : IRequest<IEnumerable<PostResponseDto>>;

}
