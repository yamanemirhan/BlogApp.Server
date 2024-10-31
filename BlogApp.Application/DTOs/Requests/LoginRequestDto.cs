using System.ComponentModel.DataAnnotations;

namespace BlogApp.Application.DTOs.Requests
{
    public class LoginRequestDto
    {
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required, MinLength(7)]
        public string Password { get; set; }
    }
}
