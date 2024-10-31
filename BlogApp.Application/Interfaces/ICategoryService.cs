using BlogApp.Application.DTOs.Responses;

namespace BlogApp.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync();
    }
}
