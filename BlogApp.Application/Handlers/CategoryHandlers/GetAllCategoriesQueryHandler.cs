using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Queries.CategoryQueries;
using MediatR;

namespace BlogApp.Application.Handlers.PostHandlers
{
    public class GetAllCategoriesQueryHandler(ICategoryService categoryService) : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
    {
        public  Task<List<CategoryDto>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
        {
            return categoryService.GetAllCategoriesAsync();
        }
    }
}
