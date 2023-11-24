using BlogApp.API.Data;
using BlogApp.API.Models.Domain;
using BlogApp.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.API.Repositories.Implementation;

public class BlogPostRepository: IBlogPostRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BlogPostRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BlogPost> CreateAsync(BlogPost blogPost)
    {
        await _dbContext.BlogPosts.AddAsync(blogPost);
        await _dbContext.SaveChangesAsync();
        return blogPost;
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await _dbContext.BlogPosts.Include(blogPost => blogPost.Categories).ToListAsync();
    }

    public async Task<BlogPost?> GetByIdAsync(Guid id)
    {
        return await _dbContext.BlogPosts.Include(blogPost => blogPost.Categories).FirstOrDefaultAsync(blogPost => blogPost.Id == id);
    }

    public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
    {
        return await _dbContext.BlogPosts.Include(blogPost => blogPost.Categories).FirstOrDefaultAsync(blogPost => blogPost.UrlHandle == urlHandle);
    }

    public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
    {
        var existingBlogPost = await _dbContext.BlogPosts.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id == blogPost.Id);
        
        if (existingBlogPost is null)
        {
            return null;
        }
        
        // Update BlogPost
        _dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);
        
        // Update Categories
        existingBlogPost.Categories = blogPost.Categories;

        await _dbContext.SaveChangesAsync();

        return blogPost;
    }

    public async Task<BlogPost?> DeleteAsync(Guid id)
    {
        var blogPost = await _dbContext.BlogPosts.FirstOrDefaultAsync(b => b.Id == id);

        if (blogPost is not null)
        {
            _dbContext.BlogPosts.Remove(blogPost);
            await _dbContext.SaveChangesAsync();
            return blogPost;
        }

        return null;
    }
}