using BlogApp.API.Models.Domain;

namespace BlogApp.API.Repositories.Interface;

public interface IImageRepository
{
    Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
}