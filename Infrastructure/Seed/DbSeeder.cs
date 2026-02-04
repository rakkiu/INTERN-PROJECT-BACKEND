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
            try
            {
                // 1. Seed Privileges
                var privileges = new[]
                {
                    new { Name = "user.create", Description = "Create users", Category = "User Management" },
                    new { Name = "user.read", Description = "View users", Category = "User Management" },
                    new { Name = "user.update", Description = "Update users", Category = "User Management" },
                    new { Name = "user.delete", Description = "Delete users", Category = "User Management" },
                    new { Name = "product.create", Description = "Create products", Category = "Product Management" },
                    new { Name = "product.read", Description = "View products", Category = "Product Management" },
                    new { Name = "product.update", Description = "Update products", Category = "Product Management" },
                    new { Name = "product.delete", Description = "Delete products", Category = "Product Management" }
                };

                foreach (var priv in privileges)
                {
                    if (!await context.Privileges.AnyAsync(p => p.Name == priv.Name))
                    {
                        await context.Privileges.AddAsync(new Privilege
                        {
                            Id = Guid.NewGuid(),
                            Name = priv.Name,
                            Description = priv.Description,
                            Category = priv.Category
                        });
                    }
                }
                await context.SaveChangesAsync();

                // 2. Seed Roles
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

                // 3. Assign Privileges to Roles
                var sellerRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Seller");
                var customerRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");

                if (sellerRole != null)
                {
                    // Seller có tất cả quyền
                    var allPrivileges = await context.Privileges.ToListAsync();
                    foreach (var privilege in allPrivileges)
                    {
                        if (!await context.RolePrivileges.AnyAsync(rp => rp.RoleId == sellerRole.Id && rp.PrivilegeId == privilege.Id))
                        {
                            await context.RolePrivileges.AddAsync(new RolePrivilege
                            {
                                Id = Guid.NewGuid(),
                                RoleId = sellerRole.Id,
                                PrivilegeId = privilege.Id
                            });
                        }
                    }
                }

                if (customerRole != null)
                {
                    // Customer chỉ có quyền đọc
                    var readPrivileges = await context.Privileges
                        .Where(p => p.Name.EndsWith(".read"))
                        .ToListAsync();
                    
                    foreach (var privilege in readPrivileges)
                    {
                        if (!await context.RolePrivileges.AnyAsync(rp => rp.RoleId == customerRole.Id && rp.PrivilegeId == privilege.Id))
                        {
                            await context.RolePrivileges.AddAsync(new RolePrivilege
                            {
                                Id = Guid.NewGuid(),
                                RoleId = customerRole.Id,
                                PrivilegeId = privilege.Id
                            });
                        }
                    }
                }
                await context.SaveChangesAsync();

                // 4. Seed Default Seller User
                var sellerEmail = "seller@rakkiu.com";
                var encryptedEmail = EncryptionHelper.EncryptDeterministic(sellerEmail);

                if (!await context.Users.AnyAsync(u => u.Email == encryptedEmail))
                {
                    if (sellerRole == null)
                    {
                        throw new InvalidOperationException("Seller role not found. Database seeding failed.");
                    }

                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "seller",
                        Email = encryptedEmail,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Seller@123"),
                        FullName = EncryptionHelper.Encrypt("Default Seller"),
                        Phone = EncryptionHelper.Encrypt("0123456789"),
                        Address = EncryptionHelper.Encrypt("System Default Address"),
                        RoleId = sellerRole.Id
                    };

                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết để debug
                Console.WriteLine($"Error seeding database: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}