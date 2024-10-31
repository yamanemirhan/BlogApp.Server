namespace BlogApp.Domain.Entities
{
    public class PostImage
    {
        public int ImageId { get; set; }
        public int PostId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
