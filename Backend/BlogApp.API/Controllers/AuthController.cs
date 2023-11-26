using BlogApp.API.Models.DTO;
using BlogApp.API.Models.Enum;
using BlogApp.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        // POST: /api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestDto requestDto)
        {
            // Check Email
            var identityUser = await _userManager.FindByEmailAsync(requestDto.Email);

            if (identityUser != null)
            {
                // Check Password
                var checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, requestDto.Password);

                if (checkPasswordResult)
                {
                    // Create Token & Response
                    var roles = await _userManager.GetRolesAsync(identityUser);

                    var jwtToken = _tokenRepository.CreateJwtToken(identityUser, roles.ToList());
                    
                    var response = new LoginResponseDto
                    {
                        Email = requestDto.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };
                    
                    return Ok(response);
                }
            }
            
            ModelState.AddModelError("", "Email or Password Incorrect");
            
            return ValidationProblem(ModelState);
        }

        // POST: /api/auth/register
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