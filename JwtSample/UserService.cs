using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtSample
{
    /// <summary>
    /// https://jwt.io
    /// </summary>

    internal class UserService
    {
        private const string SecretKey = "jkrDkfyqnnf+!RsfgrWdlfkd";

        private IDictionary<string, string> _users = new Dictionary<string, string>()
        {
            { "user1", "password1"},
            { "user2", "password2"},
            { "user3", "password3"},
            { "user4", "password4"},
            { "user5", "password5"}
        };
        
        public string Authenticate(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password)) return string.Empty;

            int userIndex = 0;

            foreach(var user in _users)
            {
                if (string.CompareOrdinal(user.Key, login) == 0 && string.CompareOrdinal(user.Value, password) == 0)
                {
                    return GenerateJwtToken(userIndex, login);
                }
                userIndex++;
            }

            return string.Empty;
        }

        private string GenerateJwtToken(int userId, string userName)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(SecretKey);
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor();
            securityTokenDescriptor.Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email,new String($"{userName}@mail.com"))
            });

            securityTokenDescriptor.Expires = DateTime.UtcNow.AddMinutes(10);
            securityTokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            SecurityToken securityToken =  jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(securityToken);

        }

    }
}
