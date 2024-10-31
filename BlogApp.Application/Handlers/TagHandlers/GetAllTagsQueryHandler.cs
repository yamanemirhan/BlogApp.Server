using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Queries.TagQueries;
using MediatR;

namespace BlogApp.Application.Handlers.TagHandlers
{
    public class GetAllTagsQueryHandler(ITagService tagService) : IRequestHandler<GetAllTagsQuery, List<TagDto>>
    {
        public  Task<List<TagDto>> Handle(GetAllTagsQuery query, CancellationToken cancellationToken)
        {
            return tagService.GetAllTagsAsync();
        }
    }
}
