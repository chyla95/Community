using Microsoft.IdentityModel.Tokens;

namespace Community.API.Utilities.Authenticator
{
    public class JwtAuthenticationConfiguration : IJwtAuthenticationConfiguration
    {
        public TokenValidationParameters TokenValidationParameters { get; private set; }

        public JwtAuthenticationConfiguration(TokenValidationParameters tokenValidationParameters)
        {
            TokenValidationParameters = tokenValidationParameters;
        }
    }
}
