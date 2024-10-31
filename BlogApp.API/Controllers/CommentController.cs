using BlogApp.Application.Commands.CommentCommands;
using BlogApp.Application.DTOs.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentRequestDto createCommentRequestDto)
        {

            var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                Console.WriteLine("User ID is missing or invalid.");
                return Unauthorized("You must log in to add a comment.");
            }

            var command = new AddCommentCommand(createCommentRequestDto, userId);

            var commentId = await mediator.Send(command);

            return CreatedAtAction(nameof(AddComment), new { id = commentId }, commentId);
        }

      
        [HttpPut("{commentId}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] UpdateCommentRequestDto updateRequest)
        {
            var command = new UpdateCommentCommand(
                CommentId: commentId,
                Content: updateRequest.Content 
                );
            var result = await mediator.Send(command);

            if (result == null)
            {
                return NotFound("Comment not found!");
            }

            return Ok(result);
        }

        [HttpDelete("{commentId}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var command = new DeleteCommentCommand(commentId);
            var isSuccess = await mediator.Send(command);
            if (!isSuccess)
            {
                return NotFound("Comment not found.");
            }
            return NoContent();
        }
    }
}
