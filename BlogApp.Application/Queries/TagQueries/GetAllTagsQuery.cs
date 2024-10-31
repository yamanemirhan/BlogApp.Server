using BlogApp.Application.DTOs.Responses;
using MediatR;

namespace BlogApp.Application.Queries.TagQueries
{
    public record GetAllTagsQuery() : IRequest<List<TagDto>>;

}
