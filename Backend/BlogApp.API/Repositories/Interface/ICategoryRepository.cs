using BlogApp.API.Models.Domain;

namespace BlogApp.API.Repositories.Interface;

public interface ICategoryRepository
{
    Task<Category> CreateAsync(Category category);
}