using BlogApp.Application.Commands.AuthCommands;
using BlogApp.Application.DTOs.Responses;
using BlogApp.Application.Helpers; // Add this to access JwtTokenGenerator
using BlogApp.Application.Services;
using BlogApp.Domain.Repositories;
using MediatR;

namespace BlogApp.Application.Handlers.AuthHandlers
{
    public class LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, JwtTokenGenerator tokenGenerator) : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Check if the username or email exists
            var user = await userRepository.GetByEmailAsync(request.LoginRequest.Email);

            if (user == null)
            {
                Console.WriteLine("User is not found!");
                throw new Exception("Invalid email or password.");
            }

            // Verify the password
            if (!passwordHasher.VerifyPassword(user.PasswordHash, request.LoginRequest.Password))
            {
                Console.WriteLine("Password is invalid!");
                throw new Exception("Invalid email or password.");
            }

            // Generate a JWT token for the user
            string token = tokenGenerator.GenerateToken(user.UserId.ToString(), user.Username, user.IsAdmin);

            return new LoginResponseDto
            {
                Token = token,
                Message = "Login successful."
            };
        }
    }
}
