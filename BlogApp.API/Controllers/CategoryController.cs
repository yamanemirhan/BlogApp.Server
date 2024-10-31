using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Queries.CategoryQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController(IMediator mediator) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var categories = await mediator.Send(query);
            if (categories == null) return NotFound();
            return Ok(categories);
        }       
    }
}
