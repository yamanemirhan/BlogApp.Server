using System.ComponentModel.DataAnnotations;

namespace BlogApp.Application.DTOs.Requests
{
        public class CreatePostRequestDto
        {
            [Required, MaxLength(200)]
            public string Title { get; set; }

            [Required, MaxLength(100)]
            public string Description { get; set; }

            [Required]
            public string Content { get; set; }

            [Required]
            public int CategoryId { get; set; }

            public string CoverImageUrl { get; set; }

            [Required]
            [MinLength(1), MaxLength(3)]
            public List<int> TagIds { get; set; } 
            public List<string> ContentImages { get; set; } = new List<string>();
        }

}
