using BlogApp.API.Models.Domain;
using BlogApp.API.Models.DTO;
using BlogApp.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        
        // GET: /api/images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetAll();
            var response = images.Select(ConvertToDto).ToList();

            return Ok(response);
        }

        // POST: /api/images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
            [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    CreationDate = DateTime.Now
                };

                blogImage = await _imageRepository.Upload(file, blogImage);

                var response = ConvertToDto(blogImage);

                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] {".jpg", ".jpeg", ".png"};

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }
        
        private static BlogImageDto ConvertToDto(BlogImage blogImage)
        {
            return new BlogImageDto
            {
                Id = blogImage.Id,
                Title = blogImage.Title,
                CreationDate = blogImage.CreationDate,
                FileExtension = blogImage.FileExtension,
                FileName = blogImage.FileName,
                Url = blogImage.Url
            };
        }
    }
}