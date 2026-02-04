using Domain.Entity;
using Infrastructure.Identity;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // 1. Seed Roles
            var roles = new[] { "Seller", "Customer" };
            foreach (var roleName in roles)
            {
                if (!await context.Roles.AnyAsync(r => r.Name == roleName))
                {
                    await context.Roles.AddAsync(new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = roleName,
                        Description = $"Default role for {roleName}"
                    });
                }
            }
            await context.SaveChangesAsync();

            // 2. Seed Default Seller User
            var sellerEmail = "seller@rakkiu.com";
            var encryptedEmail = EncryptionHelper.EncryptDeterministic(sellerEmail);

            if (!await context.Users.AnyAsync(u => u.Email == encryptedEmail))
            {
                var sellerRole = await context.Roles.FirstAsync(r => r.Name == "Seller");
                
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "seller",
                    Email = encryptedEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Seller@123"),
                    FullName = EncryptionHelper.Encrypt("Default Seller"),
                    Phone = EncryptionHelper.Encrypt("0123456789"),
                    Address = EncryptionHelper.Encrypt("System Default Address")
                };

                await context.Users.AddAsync(user);

                // 3. Assign Role
                await context.UserRoles.AddAsync(new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    RoleId = sellerRole.Id
                });

                await context.SaveChangesAsync();
            }
        }
    }
}