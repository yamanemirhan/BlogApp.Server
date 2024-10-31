using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [ApiController]
    [Route("api/image")]
    public class ImageController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _configuration;

        public ImageController(IConfiguration configuration)
        {
            _configuration = configuration;
            var cloudinaryUrl = configuration.GetSection("Cloudinary").GetSection("URL").Value;
            _cloudinary = new Cloudinary(cloudinaryUrl);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = false,
                    Folder = "blogapp"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult?.SecureUrl == null)
                {
                    return BadRequest("Upload failed, SecureUrl is null.");
                }

                return Ok(new
                {
                    SecureUrl = uploadResult.SecureUrl.ToString()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Upload error: " + ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
        }

    }

}
