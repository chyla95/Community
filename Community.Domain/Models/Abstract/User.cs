﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace Community.Domain.Models.Abstract
{
#pragma warning disable CS8618
    [Table(nameof(User) + "s")]
    public abstract class User : Entity
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(5), MaxLength(50)]
        public string Password
        {
            get { return _password; }
            set
            {
                try
                {
                    // Guard - return if given password is already hashed.
                    HashInformation hashInformation = BC.InterrogateHash(value);
                    if (!hashInformation.RawHash.IsNullOrEmpty())
                    {
                        _password = value;
                        return;
                    }
                }
                catch { }
                _password = EncryptPassword(value);
            }
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
        public string CreateJwt(string JwtSecret)
        {
            List<Claim> jwtClaims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
            };

            SymmetricSecurityKey symmetricSecurityKey = new(System.Text.Encoding.UTF8.GetBytes(JwtSecret));
            SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor securityTokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(jwtClaims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = signingCredentials
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            string jwt = jwtSecurityTokenHandler.WriteToken(securityToken);
            return jwt;
        }
    }
#pragma warning restore CS8618
}
