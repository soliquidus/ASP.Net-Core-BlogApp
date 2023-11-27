using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using BlogApp.API.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BlogApp.API.Repositories.Implementation;

public class TokenRepository: ITokenRepository
{
    private readonly IConfiguration _configuration;

    public TokenRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateJwtToken(IdentityUser user, List<string> roles)
    {
        // Create role claims
        if (user.Email != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };
        
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // JWT Security Token parameters
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? 
                                                                  throw new InvalidOperationException("JWT Key not found")));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);
        
        // Return Token
        return new JwtSecurityTokenHandler().WriteToken(token);
        }

        throw new InvalidCredentialException("Error while creating JWT Token, no corresponding user email");
    }
}