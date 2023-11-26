using BlogApp.API.Models.Domain;
using BlogApp.API.Models.DTO;
using BlogApp.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;


        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // POST: /api/categories
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto requestDto)
        {
            var category = new Category
            {
                Name = requestDto.Name,
                UrlHandle = requestDto.UrlHandle
            };

            await _categoryRepository.CreateAsync(category);

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }

        // GET: /api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var response = categories
                .Select(category => new CategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name,
                        UrlHandle = category.UrlHandle
                    }
                ).ToList();

            return Ok(response);
        }
        
        // GET: /api/categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryRepository.GetById(id);

            if (category is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }
        
        // PUT: /api/categories/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditCategory(Guid id, UpdateCategoryRequestDto requestDto)
        {
            var category = new Category
            {
                Id = id,
                Name = requestDto.Name,
                UrlHandle = requestDto.UrlHandle
            };

            category = await _categoryRepository.UpdateAsync(category);

            if (category is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = id,
                Name = requestDto.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }
        
        // DELETE: /api/categories/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _categoryRepository.DeleteAsync(id);

            if (category is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }
    }
}