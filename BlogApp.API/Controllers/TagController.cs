using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Queries.TagQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [ApiController]
    [Route("api/tag")]
    public class TagController(IMediator mediator) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<List<TagDto>>> GetAllTags()
        {
            var query = new GetAllTagsQuery();
            var tags = await mediator.Send(query);
            if (tags == null) return NotFound();
            return Ok(tags);
        }       
    }
}
