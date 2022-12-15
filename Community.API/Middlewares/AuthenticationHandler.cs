using System.IdentityModel.Tokens.Jwt;
using Community.API.Utilities.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Community.Infrastructure.Services;
using Community.Domain.Models.Abstract;
using Community.API.Utilities.Authenticator;
using System.Security.Claims;

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

        public async Task InvokeAsync(HttpContext httpContext, IUserService<User> userService, IJwtAuthenticationConfiguration jwtAuthenticationConfiguration)
        {
            if (!IsAuthorizationHeaderIncluded(httpContext))
            {
                await _next(httpContext);
                return;
            }
            string tokenString = GetAuthorizationToken(httpContext);

            JwtSecurityTokenHandler tokenHandler = new();
            try
            {
                _ = tokenHandler.ValidateToken(tokenString, jwtAuthenticationConfiguration.TokenValidationParameters, out SecurityToken validatedToken);
            }
            catch
            {
                throw new HttpUnauthorizedException("Invalid token!");
            }
            JwtSecurityToken token = tokenHandler.ReadJwtToken(tokenString);

            Claim? userIdClaim = token.Claims.SingleOrDefault(claim => claim.Type == "userId");
            if (userIdClaim == null) throw new HttpUnauthorizedException("Invalid token!");

            int userId = int.Parse(userIdClaim.Value);
            User? user = await userService.GetAsync(userId);
            if (user == null) throw new HttpUnauthorizedException("User not found!");

            httpContext.Features.Set(user);

            await _next(httpContext);
        }

        private static bool IsAuthorizationHeaderIncluded(HttpContext httpContext)
        {
            string? authorizationHeader = httpContext.Request.Headers["Authorization"];
            if(authorizationHeader.IsNullOrEmpty()) return false;
            return true;
        }

        private static string GetAuthorizationHeader(HttpContext httpContext)
        {
            string? authorizationHeader = httpContext.Request.Headers["Authorization"];
            if (authorizationHeader.IsNullOrEmpty()) throw new HttpUnauthorizedException();

            return authorizationHeader!;
        }

        private static string GetAuthorizationToken(HttpContext httpContext)
        {
            string authorizationHeader = GetAuthorizationHeader(httpContext);
            if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) throw new HttpUnauthorizedException("Invalid token!");

            string normalizedToken = authorizationHeader
                .Replace("Bearer", "", StringComparison.OrdinalIgnoreCase)
                .Trim();
            return normalizedToken!;
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
