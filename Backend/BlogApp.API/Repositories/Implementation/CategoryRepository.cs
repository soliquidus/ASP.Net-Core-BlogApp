using BlogApp.API.Data;
using BlogApp.API.Models.Domain;
using BlogApp.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.API.Repositories.Implementation;

public class CategoryRepository: ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Category> CreateAsync(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        return category;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    public async Task<Category?> GetById(Guid id)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(category => category.Id == id);
    }

    public async Task<Category?> UpdateAsync(Category category)
    {
        var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

        if (existingCategory != null)
        {
            _dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        return null;
    }

    public async Task<Category?> DeleteAsync(Guid id)
    {
       var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

       if (category is null)
       {
           return null;
       }

       _dbContext.Categories.Remove(category);
       await _dbContext.SaveChangesAsync();
       return category;
    }
}