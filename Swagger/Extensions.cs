using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FClub.Backend.Common.Swagger
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, SwaggerOptions swaggerOptions)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc(swaggerOptions.ApiVersion, new OpenApiInfo { Title = swaggerOptions.ApiTitle, Version = swaggerOptions.ApiVersion });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, Action<SwaggerOptions>? configureSwaggerOptions = null)
        {
            var swaggerOptions = new SwaggerOptions();
            configureSwaggerOptions?.Invoke(swaggerOptions);

            services.AddCustomSwagger(swaggerOptions);

            return services;
        }
    }
}
