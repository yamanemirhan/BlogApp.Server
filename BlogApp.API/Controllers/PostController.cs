using BlogApp.Application.Commands.PostCommands;
using BlogApp.Application.DTOs.Requests;
using BlogApp.Application.Queries.PostQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [ApiController]
    [Route("api/post")]
    public class PostController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequestDto createPostRequestDto)
        {
            var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                Console.WriteLine("User ID is missing or invalid.");
                return Unauthorized("You must log in to create a post.");
            }
            var command = new CreatePostCommand(createPostRequestDto, userId);
            Post createdPost = await mediator.Send(command);
            return CreatedAtAction(nameof(GetPostById), new { postId = createdPost.PostId }, createdPost);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            var query = new GetPostByIdQuery(postId);
            var post = await mediator.Send(query);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpGet("user/id/{userId}")]
        public async Task<IActionResult> GetPostsByUserId(int userId)
        {
            var query = new GetUserPostsQuery(userId);
            var posts = await mediator.Send(query);
            return Ok(posts);
        }

        [HttpGet("user/username/{username}")]
        public async Task<IActionResult> GetPostsByUsername(string username)
        {
            var query = new GetUserPostsByUsernameQuery(username);
            var posts = await mediator.Send(query);
            return Ok(posts);
        }

        [HttpPut("{postId}")]
        [Authorize]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] UpdatePostRequestDto updateRequest)
        {
            var command = new UpdatePostCommand(
                PostId: postId,
                Title: updateRequest.Title,
                Description: updateRequest.Description,
                Content: updateRequest.Content,
                CategoryId: updateRequest.CategoryId,
                CoverImageUrl: updateRequest.CoverImageUrl,
                TagIds: updateRequest.TagIds
                //UserId: User.GetUserId()
            );

            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{postId}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var command = new DeletePostCommand(postId);
            var isSuccess = await mediator.Send(command);
            if (!isSuccess)
            {
                return NotFound("Post not found.");
            }
            return NoContent();
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllPosts(
                [FromQuery] string? search, 
                [FromQuery] DateTime? fromDate, 
                [FromQuery] DateTime? toDate,
                [FromQuery] string? categoryNames,
                [FromQuery] List<string>? tagNames,
                [FromQuery] string? sortBy = "PublishedDate", 
                [FromQuery] bool isDescending = true,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 6
            )
        {
            var categoryList = categoryNames?.Split(',').ToList();
            var query = new GetAllPostsQuery(search, fromDate, toDate, categoryList, tagNames, sortBy, isDescending, pageNumber, pageSize);
            var posts = await mediator.Send(query);
            return Ok(posts);
        }
    }
}
