using BlogApp.Application.DTOs.Responses;
using MediatR;

namespace BlogApp.Application.Queries.CategoryQueries
{
    public record GetAllCategoriesQuery() : IRequest<List<CategoryDto>>;

}
