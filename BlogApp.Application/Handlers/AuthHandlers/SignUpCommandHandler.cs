using BlogApp.Application.Commands.AuthCommands;
using BlogApp.Application.Services;
using BlogApp.Domain.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Application.Handlers.AuthHandlers
{
    public class SignUpCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher) : IRequestHandler<SignUpCommand, bool>
    {
        public async Task<bool> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            // Validate password
            if (string.IsNullOrWhiteSpace(request.SignUpRequest.Password) || request.SignUpRequest.Password.Length < 7)
            {
                throw new ValidationException("Password must be at least 7 characters long and cannot be empty.");
            }

            // Check if the user already exists by email
            var existingUserByEmail = await userRepository.GetByEmailAsync(request.SignUpRequest.Email);
            if (existingUserByEmail != null)
            {
                throw new Exception("Email is already in use.");
            }

            // Check if the user already exists by username
            var existingUserByUsername = await userRepository.GetByUsernameAsync(request.SignUpRequest.Username);
            if (existingUserByUsername != null)
            {
                throw new Exception("Username is already taken.");
            }

            // Hash the user's password
            var hashedPassword = passwordHasher.HashPassword(request.SignUpRequest.Password);

            // Create a new User instance and set its properties directly
            var user = new User
            {
                Username = request.SignUpRequest.Username,
                Email = request.SignUpRequest.Email,
                PasswordHash = hashedPassword
            };

            // Use the generic repository to add the new user
            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();  // Ensure changes are committed to the database

            return true;
        }
    }
}
