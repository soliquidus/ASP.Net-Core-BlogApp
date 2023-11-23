using BlogApp.API.Models.Domain;
using BlogApp.API.Models.DTO;
using BlogApp.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostsController : Controller
{
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly ICategoryRepository _categoryRepository;

    public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
    {
        _blogPostRepository = blogPostRepository;
        _categoryRepository = categoryRepository;
    }

    // POST: /api/blogposts
    [HttpPost]
    public async Task<IActionResult> CreateBlogPost(CreateBlogPostRequestDto requestDto)
    {
        var blogPost = new BlogPost
        {
            Author = requestDto.Author,
            Content = requestDto.Content,
            FeaturedImageUrl = requestDto.FeaturedImageUrl,
            IsVisible = requestDto.IsVisible,
            PublicationDate = requestDto.PublicationDate,
            ShortDescription = requestDto.ShortDescription,
            Title = requestDto.Title,
            UrlHandle = requestDto.UrlHandle,
            Categories = new List<Category>()
        };

        foreach (var categoryGuid in requestDto.Categories)
        {
            var category = await _categoryRepository.GetById(categoryGuid);
            if (category is not null)
            {
                blogPost.Categories.Add(category);
            }
        }

        blogPost = await _blogPostRepository.CreateAsync(blogPost);

        var response = ConvertToDtoModel(blogPost);

        return Ok(response);
    }

    // GET: /api/blogposts
    [HttpGet]
    public async Task<IActionResult> GetAllBlogPosts()
    {
        var blogPosts = await _blogPostRepository.GetAllAsync();

        var response = blogPosts.Select(ConvertToDtoModel).ToList();

        return Ok(response);
    }
    
    // GET: /api/blogposts/{id}
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetBlogPostById(Guid id)
    {
        var blogPost = await _blogPostRepository.GetByIdAsync(id);

        if (blogPost is null)
        {
            return NotFound();
        }

        var response = ConvertToDtoModel(blogPost);

        return Ok(response);
    }

    // PUT: /api/blogposts/{id}
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateBlogPostById(Guid id, UpdateBlogPostRequestDto requestDto)
    {
        var blogPost = ConvertToDomainModel(id, requestDto);

        foreach (var categoryGuid in requestDto.Categories)
        {
            var category = await _categoryRepository.GetById(categoryGuid);

            if (category is not null)
            {
                blogPost.Categories.Add(category);
            }
        }
        
        var updatedBlogPost = await _blogPostRepository.UpdateAsync(blogPost);

        if (updatedBlogPost is null)
        {
            return NotFound();
        }

        var response = ConvertToDtoModel(blogPost);

        return Ok(response);
    }
    
    // DELETE: /api/blogposts/{id}
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteBlogPost(Guid id)
    {
        var deletedBlogPost = await _blogPostRepository.DeleteAsync(id);

        if (deletedBlogPost is null)
        {
            return NotFound();
        }

        var response = new BlogPostDto()
        {
            Id = deletedBlogPost.Id,
            Author = deletedBlogPost.Author,
            Content = deletedBlogPost.Content,
            FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
            IsVisible = deletedBlogPost.IsVisible,
            PublicationDate = deletedBlogPost.PublicationDate,
            ShortDescription = deletedBlogPost.ShortDescription,
            Title = deletedBlogPost.Title,
            UrlHandle = deletedBlogPost.UrlHandle,
        };

        return Ok(response);
    }

    // Utils
    private static BlogPost ConvertToDomainModel(Guid id, UpdateBlogPostRequestDto requestDto)
    {
        return new BlogPost
        {
            Id = id,
            Author = requestDto.Author,
            Content = requestDto.Content,
            FeaturedImageUrl = requestDto.FeaturedImageUrl,
            IsVisible = requestDto.IsVisible,
            PublicationDate = requestDto.PublicationDate,
            ShortDescription = requestDto.ShortDescription,
            Title = requestDto.Title,
            UrlHandle = requestDto.UrlHandle,
            Categories = new List<Category>()
        };
    }

    private static BlogPostDto ConvertToDtoModel(BlogPost blogPost)
    {
        return new BlogPostDto
        {
            Id = blogPost.Id,
            Author = blogPost.Author,
            Content = blogPost.Content,
            FeaturedImageUrl = blogPost.FeaturedImageUrl,
            IsVisible = blogPost.IsVisible,
            PublicationDate = blogPost.PublicationDate,
            ShortDescription = blogPost.ShortDescription,
            Title = blogPost.Title,
            UrlHandle = blogPost.UrlHandle,

            Categories = blogPost.Categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                UrlHandle = c.UrlHandle
            }).ToList()
        };
    }
}