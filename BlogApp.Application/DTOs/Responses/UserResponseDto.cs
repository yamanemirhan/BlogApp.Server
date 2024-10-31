namespace BlogApp.Application.DTOs.Responses
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        //Posts
        //Comments
    }
}
