using Data.Layer.Contexts;
using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Repository.Layer
{
    public class IdentityContextSeed
    {

        private static async Task<List<T>> LoadSeedDataAsync<T>(string fileName)
        {
            var filePath = $"../Repository.Layer/SeedData/{fileName}";
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
        private static async Task SeedUsersAsync(UserManager<AppUser> userManager, List<SeedUser> seedUsers, RoleManager<IdentityRole> _roleManager, ILoggerFactory loggerFactory)
        {
            try
            {
                if (await userManager.Users.AnyAsync()) return; // Skip if users already exist

                foreach (var seedUser in seedUsers)
                {
                    var user = new AppUser
                    {
                        UserName = seedUser.UserName,
                        Email = seedUser.Email,
                        DisplayName = seedUser.DisplayName,
                        Address = seedUser.Address,
                    };
                    await userManager.CreateAsync(user, seedUser.Password); // Create user with password

                    foreach (var role in seedUser.Roles)
                    {
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(role));
                        }

                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<IdentityContextSeed>();
                logger.LogError(ex.Message, "An error occurred while seeding the database.");
            }
        }
        public static async Task SeedAsync(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> _roleManager, ILoggerFactory loggerFactory)
        {
            // Seed Users
            try
            {
                var users = await LoadSeedDataAsync<SeedUser>("usersSeedData.json");
                await SeedUsersAsync(userManager, users, _roleManager, loggerFactory);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<IdentityContextSeed>();
                logger.LogError(ex.Message, "An error occurred while seeding the database.");
            }
        }
    }
}
