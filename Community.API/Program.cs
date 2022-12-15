
using System.Text;
using Community.API.Extensions;
using Community.API.Filters;
using Community.API.Middlewares;
using Community.API.Utilities;
using Community.API.Utilities.Accessors;
using Community.API.Utilities.Authenticator;
using Community.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Community.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Get application settings
            string? jwtSecretKey = builder.Configuration.GetSection(Configuration.JWT_SECRET_KEY).Value;
            if (jwtSecretKey == null) throw new NullReferenceException(nameof(jwtSecretKey));
            string? dbConnectionString = builder.Configuration.GetSection(Configuration.DB_CONNECTION_STRING_KEY).Value;
            if (dbConnectionString == null) throw new NullReferenceException(nameof(dbConnectionString));

            // Add services to the container.
            TokenValidationParameters tokenValidationParameters = new()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
            };
            JwtAuthenticationConfiguration authenticationConfiguration = new(tokenValidationParameters);
            builder.Services.AddJwtAuthentication(authenticationConfiguration);
            builder.Services.AddSwaggerGen(configuration =>
            {
                configuration.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Scheme = "Bearer",
                    Description = "Standard Authorization header using the bearer scheme, e.g. \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                configuration.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IContextAccessor, ContextAccessor>();
            builder.Services.AddScoped<ISettingsAccessor, SettingsAccessor>();

            builder.Services.AddInfrastructureServices(dbConnectionString);
            builder.Services.AddAutoMapper(typeof(Program));

            // Configure the HTTP request pipeline.
            WebApplication app = builder.Build();
            if (app.Environment.IsDevelopment()) app.UseSwagger();
            if (app.Environment.IsDevelopment()) app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.SetupExceptionHandler();
            app.SetupAuthenticationHandler();
            app.MapControllers();
            app.Run();
        }
    }
}