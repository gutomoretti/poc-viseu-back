

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using PocViseu.Api.Jwt.Interfaces;
using PocViseu.Api.Jwt.Model;

namespace PocViseu.Api.Jwt
{
    public class TokenRefresher : ITokenRefresher
    {
        private readonly byte[] key;
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        public TokenRefresher(byte[] key, IJWTAuthenticationManager jWTAuthenticationManager)
        {
            this.key = key;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }

        public AuthenticationResponse Refresh(RefreshCredential refreshCred)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            var pricipal = tokenHandler.ValidateToken(refreshCred.JwtToken,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
                }, out validatedToken);
            var jwtToken = validatedToken as JwtSecurityToken;
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token passed!");
            }

            var userName = pricipal.Identity.Name;
            if (refreshCred.RefreshToken != jWTAuthenticationManager.UsersRefreshTokens[userName])
            {
                throw new SecurityTokenException("Invalid token passed!");
            }

            return jWTAuthenticationManager.Authenticate(userName, pricipal.Claims.ToArray());
        }
    }
}
