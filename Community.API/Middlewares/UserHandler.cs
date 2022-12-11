using System.Security.Claims;
using Community.Domain.Models;
using Community.Infrastructure.Services;

namespace Community.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class UserHandler
    {
        private readonly RequestDelegate _next;

        public UserHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IUserService<User> userService)
        {
            string? userIdClaimValue = httpContext.User.FindFirstValue("userId");
            if (!string.IsNullOrEmpty(userIdClaimValue))
            {
                int userId = int.Parse(userIdClaimValue);
                User? user = await userService.GetOneAsync(userId);
                if (user == null) throw new Exception($"User with ID of {userId} does not exist!");

                httpContext.Features.Set(user);
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseUserHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserHandler>();
        }
    }
}
