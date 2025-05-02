using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
namespace velora.api.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Velora API", Version = "v1" });
                options.UseInlineDefinitionsForEnums();

                var SecurityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Enter 'Bearer {token}' (include the word 'Bearer' and a space before the token)",

                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition("Bearer", SecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        SecurityScheme,
                        new string[] {}
                    }
                });
            });

            return services;
        }
    }
}
