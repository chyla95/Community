using System.IdentityModel.Tokens.Jwt;
using Community.API.Utilities.Exceptions;
using Microsoft.IdentityModel.Tokens;
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

        public async Task InvokeAsync(HttpContext httpContext, IJwtAuthenticationConfiguration jwtAuthenticationConfiguration)
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
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(tokenString, jwtAuthenticationConfiguration.TokenValidationParameters, out SecurityToken validatedToken);
                httpContext.User = claimsPrincipal;
            }
            catch
            {
                throw new HttpUnauthorizedException("Invalid token!");
            }

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
