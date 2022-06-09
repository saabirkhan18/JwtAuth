using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Models
{
    public class JwtAuthenticationManager : IjwtAuthenticationManager
    {
        private readonly string Key;
        public JwtAuthenticationManager(string Key)
        {
            this.Key = Key;
        }



        public string Authenticate(string username, string password)
        {
            if (!(username == "saabir" && password == "123456"))
            {
                return null;
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes(Key);
                var TokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]{
                        new Claim(ClaimTypes.Name,username)
                    }),
                    Expires = DateTime.Now.AddMinutes(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                    )
                };
                var token = tokenHandler.CreateToken(TokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        }
    }
}
