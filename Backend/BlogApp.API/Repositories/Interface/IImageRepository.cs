using BlogApp.API.Models.Domain;

namespace BlogApp.API.Repositories.Interface;

public interface IImageRepository
{
    Task<IEnumerable<BlogImage>> GetAll();
    Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
}