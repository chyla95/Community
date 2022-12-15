using Microsoft.IdentityModel.Tokens;

namespace Community.API.Utilities.Authenticator
{
    public interface IJwtAuthenticationConfiguration
    {
        TokenValidationParameters TokenValidationParameters { get; }
    }
}
