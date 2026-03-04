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
                // 1. Seed Roles (ADMIN, LEADER, MEMBER)
                var roles = new[] 
                { 
                    new { Name = RoleNames.ADMIN },
                    new { Name = RoleNames.LEADER },
                    new { Name = RoleNames.MEMBER }
                };
                
                foreach (var roleData in roles)
                {
                    if (!await context.Roles.AnyAsync(r => r.Name == roleData.Name))
                    {
                        await context.Roles.AddAsync(new Role
                        {
                            Id = Guid.NewGuid(),
                            Name = roleData.Name,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
                await context.SaveChangesAsync();

                // 2. Seed Default Admin User
                var adminEmail = "admin@rakkiu.com";
                var encryptedAdminEmail = EncryptionHelper.EncryptDeterministic(adminEmail);
                
                if (!await context.Users.AnyAsync(u => u.Email == encryptedAdminEmail))
                {
                    var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == RoleNames.ADMIN);
                    if (adminRole == null)
                    {
                        throw new InvalidOperationException("Admin role not found. Database seeding failed.");
                    }

                    var adminUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = encryptedAdminEmail,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                        FullName = EncryptionHelper.Encrypt("System Administrator"),
                        IsActive = true,
                        RoleId = adminRole.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await context.Users.AddAsync(adminUser);
                    await context.SaveChangesAsync();
                }

                // 3. Seed Default Leader User
                var leaderEmail = "leader@rakkiu.com";
                var encryptedLeaderEmail = EncryptionHelper.EncryptDeterministic(leaderEmail);
                
                if (!await context.Users.AnyAsync(u => u.Email == encryptedLeaderEmail))
                {
                    var leaderRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == RoleNames.LEADER);
                    if (leaderRole != null)
                    {
                        var leaderUser = new User
                        {
                            Id = Guid.NewGuid(),
                            Email = encryptedLeaderEmail,
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Leader@123"),
                            FullName = EncryptionHelper.Encrypt("Team Leader"),
                            IsActive = true,
                            RoleId = leaderRole.Id,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        await context.Users.AddAsync(leaderUser);
                        await context.SaveChangesAsync();
                    }
                }

                // 4. Seed Default Member User
                var memberEmail = "member@rakkiu.com";
                var encryptedMemberEmail = EncryptionHelper.EncryptDeterministic(memberEmail);
                
                if (!await context.Users.AnyAsync(u => u.Email == encryptedMemberEmail))
                {
                    var memberRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == RoleNames.MEMBER);
                    if (memberRole != null)
                    {
                        var memberUser = new User
                        {
                            Id = Guid.NewGuid(),
                            Email = encryptedMemberEmail,
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Member@123"),
                            FullName = EncryptionHelper.Encrypt("Team Member"),
                            IsActive = true,
                            RoleId = memberRole.Id,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        await context.Users.AddAsync(memberUser);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding database: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}