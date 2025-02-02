using CleanSharpArchitecture.Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace CleanSharpArchitecture.API.Extensions;

public static class JwtConfiguration
{
    public static IServiceCollection AddJwtConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettingsSection = configuration.GetSection("JwtSettings");
        services.Configure<JwtSettings>(jwtSettingsSection);
        var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

        var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // Defina como true em produção
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero // sem tolerância para expiração
            };
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Sua API", Version = "v1" });

            // Definindo o esquema de segurança do JWT
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta forma: Bearer {seu token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            // Requisitando a utilização do esquema de segurança em todas as operações
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
        });

        return services;
    }
}