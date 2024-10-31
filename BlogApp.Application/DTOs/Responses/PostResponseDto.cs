using BlogApp.Domain.Entities;

namespace BlogApp.Application.DTOs.Responses
{
    public class PostResponseDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string CoverImageUrl { get; set; }

        public AuthorDto Author { get; set; }
        
        public CategoryDto Category { get; set; }

        public List<TagDto> Tags { get; set; }

        public List<CommentDto> Comments { get; set; }
        public ICollection<PostImage> PostImages { get; set; } = new List<PostImage>();

    }

    public class AuthorDto
    {
        public int AuthorId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }

    public class TagDto
    {
        public int TagId { get; set; }
        public string Name { get; set; }
    }

    // New CommentDto class
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public AuthorDto Author { get; set; }

        public bool IsUpdated { get; set; }
        public List<CommentDto> Children { get; set; } // To handle nested comments
    }
}
