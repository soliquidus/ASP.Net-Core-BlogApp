using BlogApp.API.Models.DTO;
using BlogApp.API.Models.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequestDto requestDto)
        {
            // Create IdentityUser object
            var user = new IdentityUser
            {
                UserName = requestDto.Email?.Trim(),
                Email = requestDto.Email?.Trim()
            };

            // Create User
            var identityResult = await _userManager.CreateAsync(user, requestDto.Password);

            if (identityResult.Succeeded)
            {
                // Add Role to user (Reader)
                await _userManager.AddToRoleAsync(user, UserRoles.Reader.GetDescription());

                if (identityResult.Succeeded)
                {
                    return Ok();
                }

                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }
    }
}