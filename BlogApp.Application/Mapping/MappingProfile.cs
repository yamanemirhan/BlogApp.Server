using AutoMapper;
using BlogApp.Application.Queries.PostQueries;
using BlogApp.Domain.DTOs.Requests;

namespace BlogApp.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetAllPostsQueryDto, GetAllPostsQuery>();
        }
    }
}
