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
    public interface IjwtAuthenticationManager
    {
        public AuthenticationResponse Authenticate(string username, string password);
        public IDictionary<string, string> UserRefreshToken { get; }
        public AuthenticationResponse Authenticate(string username, Claim[] claims);
    }
    public class JwtAuthenticationManager : IjwtAuthenticationManager
    {
        private readonly string Key;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;
        public IDictionary<string, string> UserRefreshToken = new Dictionary<string, string>();


        public JwtAuthenticationManager(string Key, IRefreshTokenGenerator refreshTokenGenerator)
        {
            this.Key = Key;
            this.refreshTokenGenerator = refreshTokenGenerator;
        }

        IDictionary<string, string> IjwtAuthenticationManager.UserRefreshToken => throw new NotImplementedException();

        public AuthenticationResponse Authenticate(string username, Claim[] claims)
        {
            var tokenKey = Encoding.ASCII.GetBytes(Key);
            var jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                 expires: DateTime.UtcNow.AddMinutes(30),
                  signingCredentials: new SigningCredentials(
                      new SymmetricSecurityKey(tokenKey),
                       SecurityAlgorithms.HmacSha256Signature)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = refreshTokenGenerator.GenerateToken();
            if (UserRefreshToken.ContainsKey(username))
            {
                UserRefreshToken[username] = refreshToken;
            }
            else
            {
                UserRefreshToken.Add(username, refreshToken);
            }
            UserRefreshToken.Add(username, refreshToken);

            return new AuthenticationResponse()
            {
                jwt_token = token,
                refresh_token = refreshToken
            };
        }
        public AuthenticationResponse Authenticate(string username, string password)
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
                    Expires = DateTime.Now.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                    )
                };
                var token = tokenHandler.CreateToken(TokenDescriptor);
                var refreshToken = refreshTokenGenerator.GenerateToken();

                if (UserRefreshToken.ContainsKey(username))
                {
                    UserRefreshToken[username] = refreshToken;
                }
                else
                {
                    UserRefreshToken.Add(username, refreshToken);
                }

                return new AuthenticationResponse()
                {
                    jwt_token = tokenHandler.WriteToken(token),
                    refresh_token = refreshToken
                };
            }
        }
    }
}
