using AutoMapper;
using Data.Layer.Contexts;
using Microsoft.EntityFrameworkCore;
using Repository.Layer;
using Repository.Layer.Interfaces;
using Services.Layer.Profiles;
using Services.Layer.Services;
using Services.Layer.Services.Token;
using Services.Layer.Services.User;

namespace Playground_Environment.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Add services to the container.
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(TicketProfile).Assembly);
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITicketService, TicketService>();
            return services;
        }
    }
}
