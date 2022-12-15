using Community.API.Utilities.Authenticator;

namespace Community.API.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtAuthenticationConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            services.AddScoped<IJwtAuthenticationConfiguration, JwtAuthenticationConfiguration>(_ => configuration);

            return services;
        }
    }
}
