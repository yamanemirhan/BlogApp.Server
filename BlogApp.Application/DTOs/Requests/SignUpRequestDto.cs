using System.ComponentModel.DataAnnotations;

namespace BlogApp.Application.DTOs.Requests
{
    public class SignUpRequestDto
    {
        [Required, MinLength(3), MaxLength(50)]
        public string Username { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required, MinLength(7)]
        public string Password { get; set; }
    }
}
