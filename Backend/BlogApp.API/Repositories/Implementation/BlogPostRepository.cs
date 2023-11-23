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
}