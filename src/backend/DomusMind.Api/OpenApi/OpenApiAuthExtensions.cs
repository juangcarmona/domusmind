using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DomusMind.Api.OpenApi;

public static class OpenApiAuthExtensions
{
    /// <summary>
    /// Adds Swagger/OpenAPI with JWT bearer support.
    /// The bearer authorize button lets testers paste a token obtained from POST /api/auth/login.
    /// </summary>
    public static IServiceCollection AddDomusMindOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DomusMind API",
                Version = "v1",
                Description = "Auth endpoints: POST /api/auth/register → POST /api/auth/login → copy AccessToken → Authorize.",
            });

            var bearerScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "Paste your JWT access token here. " +
                    "Obtain one via POST /api/auth/login.",
            };

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, bearerScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme,
                        },
                    },
                    Array.Empty<string>()
                },
            });
        });

        return services;
    }
}
