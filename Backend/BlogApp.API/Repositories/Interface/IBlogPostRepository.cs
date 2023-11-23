using BlogApp.API.Models.Domain;

namespace BlogApp.API.Repositories.Interface;

public interface IBlogPostRepository
{
    Task<BlogPost> CreateAsync(BlogPost blogPost);
    Task<IEnumerable<BlogPost>> GetAllAsync();
}