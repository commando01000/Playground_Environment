using Data.Layer.Contexts;
using Data.Layer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Repository.Layer
{
    public class ApplicationContextSeed
    {
        private static async Task<List<T>> LoadSeedDataAsync<T>(string fileName)
        {
            var filePath = $"../Repository.Layer/SeedData/{fileName}";
            var json = await File.ReadAllTextAsync(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() } // Add this to handle enums as strings
            };
            return JsonSerializer.Deserialize<List<T>>(json, options);
        }
        private static async Task SeedTicketsAsync(AppDbContext context, List<Ticket> seedTickets, ILoggerFactory loggerFactory)
        {
            try
            {
                if (await context.Tickets.AnyAsync()) return; // Skip if tickets already exist
               
                var users = await context.Users.ToListAsync(); // Get all users

                foreach (var seedTicket in seedTickets)
                {
                    // Find the user by UserName and Assign the ticket to a random user

                    var random = new Random();
                    var randomIndex = random.Next(users.Count);
                    seedTicket.User = users[randomIndex];

                    var user = users.FirstOrDefault(u => u.UserName == seedTicket.User.UserName);

                    if (user == null)
                    {
                        throw new Exception($"User with UserName '{seedTicket.User.UserName}' not found.");
                    }

                    var ticket = new Ticket
                    {
                        Title = seedTicket.Title,
                        Description = seedTicket.Description,
                        Status = seedTicket.Status, // Convert string to enum
                        Priority = seedTicket.Priority,
                        UserId = user.Id,
                    };

                    await context.Tickets.AddAsync(ticket);
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationContextSeed>();
                logger.LogError(ex.Message, "An error occurred while seeding the database.");
            }
        }
        public static async Task SeedAsync(AppDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                // Seed Tickets
                var tickets = await LoadSeedDataAsync<Ticket>("ticketsSeedData.json");
                await SeedTicketsAsync(context, tickets, loggerFactory);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationContextSeed>();
                logger.LogError(ex.Message, "An error occurred while seeding the database.");
            }
        }
    }
}
