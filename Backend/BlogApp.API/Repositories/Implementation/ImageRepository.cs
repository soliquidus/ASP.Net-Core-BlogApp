using BlogApp.API.Data;
using BlogApp.API.Models.Domain;
using BlogApp.API.Repositories.Interface;

namespace BlogApp.API.Repositories.Implementation;

public class ImageRepository : IImageRepository
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _dbContext;

    public ImageRepository(IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext dbContext)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
    {
        // Upload the image to API/Images
        var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");

        await using var stream = new FileStream(localPath, FileMode.Create);
        await file.CopyToAsync(stream);

        // Update the database
        var httpRequest = _httpContextAccessor.HttpContext?.Request;
        // Build url address
        if (httpRequest != null)
        {
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;
        }
        
        await _dbContext.BlogImages.AddAsync(blogImage);
        await _dbContext.SaveChangesAsync();

        return blogImage;
    }
}