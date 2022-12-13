using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace Community.Domain.Models.Abstract
{
#pragma warning disable CS8618
    public abstract class User : Entity
    {
        [Required]
        [MinLength(5), MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MinLength(5), MaxLength(100)]
        public string Password
        {
            get { return _password; }
            set { _password = EncryptPassword(value); }
        }
        private string _password;

        private static string EncryptPassword(string password)
        {
            string encryptedPassword = BC.HashPassword(password);
            return encryptedPassword;
        }
        public bool ComparePassword(string password)
        {
            bool doesPasswordMatch = BC.Verify(password, Password);
            return doesPasswordMatch;
        }
        public string CreateJwtToken(string JwtTokenSecret)
        {
            List<Claim> jwtTokenClaims = new()
            {
                new Claim("userId", Id.ToString()),
            };

            SymmetricSecurityKey symmetricSecurityKey = new(System.Text.Encoding.UTF8.GetBytes(JwtTokenSecret));
            SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor securityTokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(jwtTokenClaims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = signingCredentials
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            string JwtToken = jwtSecurityTokenHandler.WriteToken(securityToken);
            return JwtToken;
        }
    }
#pragma warning restore CS8618
}
