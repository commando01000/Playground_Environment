using Microsoft.OpenApi.Models;

namespace Playground_Environment.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Playground API",
                    Version = "v1",
                    Description = "API for playground environment",
                    Contact = new OpenApiContact
                    {
                        Name = "Supraa",
                        Email = "jfijcc124@gmail.com",
                        Url = new Uri("https://localhost:7154/")
                    }
                });
                options.AddServer(new OpenApiServer
                {
                    Url = "https://localhost:7154/",
                    Description = "Playground Server"
                });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                     BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme,
                    }
                };
                options.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };
                options.AddSecurityRequirement(securityRequirement);
            });
            return services;
        }
    }
}
