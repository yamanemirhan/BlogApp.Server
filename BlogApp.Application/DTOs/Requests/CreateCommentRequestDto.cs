namespace BlogApp.Application.DTOs.Requests
{
    public class CreateCommentRequestDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public int? ParentCommentId { get; set; } 
    }

}
