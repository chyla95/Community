using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Community.API.Utilities.Exceptions;
using Community.API.Utilities;
using Microsoft.IdentityModel.Tokens;
using Community.API.Utilities.Accessors;
using Community.Domain.Models;
using Community.Infrastructure.Services;

namespace Community.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthenticationHandler
    {
        private readonly RequestDelegate _next;

        public AuthenticationHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IUserService<User> userService, ISettingsAccessor settingsAccessor)
        {
            string? authorizationHeader = httpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                await _next(httpContext);
                return;
            }
            if (!authorizationHeader.ToLower().StartsWith("bearer"))
            {
                throw new HttpUnauthorizedException("Invalid token!");
            }

            string token = authorizationHeader.Replace("Bearer", "", StringComparison.OrdinalIgnoreCase).Trim();
            string secret = settingsAccessor.GetValue(Configuration.JWT_SECRET_KEY);

            JwtSecurityTokenHandler tokenHandler = new();
            TokenValidationParameters validationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };

            try
            {
                _ = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            }
            catch
            {
                throw new HttpUnauthorizedException("Invalid token!");
            }

            JwtSecurityToken jwt = tokenHandler.ReadJwtToken(token);

            string userId = jwt.Claims.First(claim => claim.Type == "userId").Value;
            if (!string.IsNullOrEmpty(userId))
            {
                User? user = await userService.GetAsync(int.Parse(userId));
                if (user == null) throw new NullReferenceException(nameof(user));

                httpContext.Features.Set(user);
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationHandlerExtensions
    {
        public static IApplicationBuilder SetupAuthenticationHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationHandler>();
        }
    }
}
