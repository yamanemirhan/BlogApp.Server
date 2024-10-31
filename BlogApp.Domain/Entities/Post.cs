using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BlogApp.Domain.Entities;
using System.Text.Json.Serialization;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PostId { get; set; }

    [Required, MaxLength(50)]
    public string Title { get; set; }

    [Required, MaxLength(100)]
    public string Description { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime PublishedDate { get; set; } = DateTime.Now;
    public DateTime? LastUpdated { get; set; } = DateTime.Now;

    [ForeignKey("User")]
    [Required]
    public int AuthorId { get; set; }
    public User Author { get; set; }

    [ForeignKey("Category")]
    [Required]
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public string CoverImageUrl { get; set; } = "https://res.cloudinary.com/dcj1bb8vk/image/upload/v1728852590/blogapp/default-post_celfxg.png";

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [JsonIgnore]
    public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    [JsonIgnore]
    public ICollection<PostImage> PostImages { get; set; } = new List<PostImage>();

}
