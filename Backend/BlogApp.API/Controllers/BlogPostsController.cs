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

    public BlogPostsController(IBlogPostRepository blogPostRepository)
    {
        _blogPostRepository = blogPostRepository;
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
            UrlHandle =  requestDto.UrlHandle
        };

        blogPost = await _blogPostRepository.CreateAsync(blogPost);

        var response = new BlogPostDto
        {
            Id = blogPost.Id,
            Author = blogPost.Author,
            Content = blogPost.Content,
            FeaturedImageUrl = blogPost.FeaturedImageUrl,
            IsVisible = blogPost.IsVisible,
            PublicationDate = blogPost.PublicationDate,
            ShortDescription = blogPost.ShortDescription,
            Title = blogPost.Title,
            UrlHandle = blogPost.UrlHandle
        };

        return Ok(response);
    }
    
    // GET: /api/blogposts
    [HttpGet]
    public async Task<IActionResult> GetAllBlogPosts()
    {
        var blogPosts = await _blogPostRepository.GetAllAsync();

        var response = blogPosts.Select(blogPost => new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublicationDate = blogPost.PublicationDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle
            })
            .ToList();

        return Ok(response);
    }
}