using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Domain.Repositories;

namespace BlogApp.Infrastructure.Services
{
    public class CategoryService(
        ICategoryRepository categoryRepository
) : ICategoryService
    {
        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await categoryRepository.GetAllAsync();

            var categoryDtos = categories.Select(category => new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            }).ToList();

            return categoryDtos;

        }
    }
}
