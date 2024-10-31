using BlogApp.API.Extensions;
using BlogApp.Application.Commands.AuthCommands;
using BlogApp.Application.DTOs.Requests;
using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{


    [ApiController]
    [Route("api/auth")]
    public class AuthController(IMediator mediator, ITokenBlacklistService tokenBlacklistService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var command = new LoginCommand(loginRequest);
                var result = await mediator.Send(command);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                };

                Response.Cookies.Append("auth_token", result.Token, cookieOptions);

                return Ok(new { Message = "Login successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto signUpRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           try
           {
                var command = new SignUpCommand(signUpRequest);
                var result = await mediator.Send(command);
                return Ok("User created successfully.");
            }
           catch (Exception ex)
           {
                return BadRequest(new { message = ex.Message });
           }
        }

        [HttpPost("logout")]
        [Authorize] // Ensures the user is authenticated
        public IActionResult Logout()
        {
            //var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //Console.WriteLine("TOKEN: " + token);
            //if (string.IsNullOrEmpty(token)) return BadRequest("No token provided.");

            //tokenBlacklistService.AddToBlacklist(token);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,        
                Secure = true,        
                SameSite = SameSiteMode.None, 
                Expires = DateTime.UtcNow.AddDays(7) 
            };
            Response.Cookies.Delete("auth_token", cookieOptions);
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<ChangePasswordResponseDto>> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            var userId = User.GetUserId();
            var command = new ChangePasswordCommand(userId, request.OldPassword, request.NewPassword);
            var response = await mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }
    }
}
