using BlogApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TagId { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; }

    public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();

}

