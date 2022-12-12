
using Community.API.Middlewares;
using Community.API.Utilities;
using Community.API.Utilities.Accessors;
using Community.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

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
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            builder.Services.AddSwaggerGen(configuration =>
            {
                configuration.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the bearer scheme, e.g. \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                configuration.OperationFilter<SecurityRequirementsOperationFilter>();
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
            app.SetupExceptionHandler();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseUserHandler();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}