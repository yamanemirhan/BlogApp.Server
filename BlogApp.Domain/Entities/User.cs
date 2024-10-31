using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int UserId { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; }

    [Required, MaxLength(100)]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    public bool IsAdmin { get; set; } = false;

    public string ProfileImageUrl { get; set; } = "https://res.cloudinary.com/dcj1bb8vk/image/upload/v1728852537/blogapp/default-user_uo85fb.jpg";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }
}
