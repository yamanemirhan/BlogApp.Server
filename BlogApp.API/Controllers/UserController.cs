using BlogApp.API.Extensions;
using BlogApp.Application.Commands.UserCommand;
using BlogApp.Application.DTOs.Requests;
using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> GetCurrentUserProfile()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var query = new GetCurrentUserProfileQuery(userId);
            var userProfile = await mediator.Send(query);

            if (userProfile == null)
            {
                return NotFound();
            }

            return Ok(userProfile);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserResponseDto>> GetUserProfileByUsername(string username)
        {
            var query = new GetUserProfileByUsernameQuery(username);
            var userProfile = await mediator.Send(query);

            if (userProfile == null)
            {
                return NotFound();
            }

            return Ok(userProfile);
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> UpdateUserProfile(
            [FromBody] UpdateUserProfileRequestDto request)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                var command = new UpdateUserProfileCommand(
                    userId,
                    request.Username,
                    request.ProfileImageUrl
                );

                var result = await mediator.Send(command);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while updating the profile." });
            }
        }

    }
}
