using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Domain.Repositories;

namespace BlogApp.Infrastructure.Services
{
    public class TagService(
        ITagRepository tagRepository
) : ITagService
    {
        public async Task<List<TagDto>> GetAllTagsAsync()
        {
            var tags = await tagRepository.GetAllAsync();

            var tagDtos = tags.Select(tag => new TagDto
            {
                TagId = tag.TagId,
                Name = tag.Name
            }).ToList();

            return tagDtos;

        }
    }
}
