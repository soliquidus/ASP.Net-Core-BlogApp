using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.API.Data;

public class AuthDbContext : IdentityDbContext
{
    private readonly IConfiguration _configuration;

    public AuthDbContext(DbContextOptions<AuthDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var readerRoleId = _configuration["DatabaseRoles:readerRoleId"];
        var writerRoleId = _configuration["DatabaseRoles:writerRoleId"];

        // Create Reader and Writer role
        if (readerRoleId != null && writerRoleId != null)
        {
            var roles = new List<IdentityRole>
            {
                new()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            // Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);
        }

        // Create Admin User
        var adminUserId = _configuration["DatabaseUsers:admin:id"];
        if (adminUserId != null)
        {
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = _configuration["DatabaseUsers:admin:userName"],
                Email = _configuration["DatabaseUsers:admin:email"],
                NormalizedEmail = _configuration["DatabaseUsers:admin:email"]?.ToUpper(),
                NormalizedUserName = _configuration["DatabaseUsers:admin:id"]?.ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, _configuration["DatabaseUsers:admin:password"] ??
                                                                                        throw new InvalidOperationException("No admin password found"));

            builder.Entity<IdentityUser>().HasData(admin);
        }

        // Give role to admin
        if (adminUserId != null && readerRoleId != null && writerRoleId != null)
        {
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}