using BlogApp.Application.DTOs.Responses;

namespace BlogApp.Application.Interfaces
{
    public interface ITagService
    {
        Task<List<TagDto>> GetAllTagsAsync();
    }
}
