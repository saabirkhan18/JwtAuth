using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JwtAuth.Models
{
    public interface ITokenRefresher
    {
        AuthenticationResponse Refresh(RefreshTokenCred refreshTokenCred);
    }
    public class TokenRefresher : ITokenRefresher
    {
        private readonly byte[] key;
        private readonly IjwtAuthenticationManager jwtAuthenticationManager;
        public TokenRefresher(byte[] key, IjwtAuthenticationManager jwtAuthenticationManager)
        {
            this.key = key;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }
        public AuthenticationResponse Refresh(RefreshTokenCred refreshTokenCred)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validateToken;
            var principal = tokenHandler.ValidateToken(refreshTokenCred.jwt_token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                },
                out validateToken);
            var jwtToken = validateToken as JwtSecurityToken;
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new SecurityTokenException("Invalid jwt token");
            }
            var userName = principal.Identity.Name;
            if (refreshTokenCred.refresh_token != jwtAuthenticationManager.UserRefreshToken[userName])
            {
                throw new SecurityTokenException("Invalid refresh token");
            }
            return jwtAuthenticationManager.Authenticate(userName, principal.Claims.ToArray());
        }
    }
}


