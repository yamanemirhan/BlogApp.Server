using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int CommentId { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsUpdated { get; set; } = false;

    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }

    [ForeignKey("Post")]
    public int PostId { get; set; }
    public Post Post { get; set; }

    // Self-referencing foreign key for replies
    public int? ParentCommentId { get; set; } 
    [ForeignKey("ParentCommentId")]
    public Comment ParentComment { get; set; }  

    // Navigation property to child comments (replies)
    public ICollection<Comment> Replies { get; set; } 
}
