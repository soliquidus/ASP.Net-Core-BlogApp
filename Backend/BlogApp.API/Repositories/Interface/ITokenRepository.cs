using Microsoft.AspNetCore.Identity;

namespace BlogApp.API.Repositories.Interface;

public interface ITokenRepository
{
    string CreateJwtToken(IdentityUser user, List<string> roles);
}