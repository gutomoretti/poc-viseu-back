using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PocViseu.Api.Jwt.Interfaces;
using PocViseu.Api.Jwt.Model;
using PocViseu.Model.Auth;

namespace PocViseu.Api.Jwt
{
    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {

        public IDictionary<string, string> UsersRefreshTokens { get; set; }

        private readonly string tokenKey;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;

        public JWTAuthenticationManager(string tokenKey, IRefreshTokenGenerator refreshTokenGenerator)
        {
            this.tokenKey = tokenKey;
            this.refreshTokenGenerator = refreshTokenGenerator;
            UsersRefreshTokens = new Dictionary<string, string>();
        }

        public AuthenticationResponse Authenticate(string username, Claim[] claims)
        {
            var token = GenerateTokenString(0, username, string.Empty, DateTime.UtcNow, claims);
            var refreshToken = refreshTokenGenerator.GenerateToken();

            if (UsersRefreshTokens.ContainsKey(username))
            {
                UsersRefreshTokens[username] = refreshToken;
            }
            else
            {
                UsersRefreshTokens.Add(username, refreshToken);
            }

            return new AuthenticationResponse
            {
                JwtToken = token,
                RefreshToken = refreshToken
            };
        }

        public AuthenticationResponse Authenticate(User user)
        {
            var token = GenerateTokenString(user.Id, user.Username!, user.Role!, DateTime.UtcNow);
            var refreshToken = refreshTokenGenerator.GenerateToken();

            if (UsersRefreshTokens.ContainsKey(user.Username!))
            {
                UsersRefreshTokens[user.Username] = refreshToken;
            }
            else
            {
                UsersRefreshTokens.Add(user.Username, refreshToken);
            }

            return new AuthenticationResponse
            {
                Id = user.Id,
                JwtToken = token,
                RefreshToken = refreshToken,
                Role = user.Role,
                Name = user.Username,
                Fullname = user.NomeCompleto
            };
        }

        string GenerateTokenString(long id, string username, string role, DateTime expires, Claim[] claims = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                 claims ?? new Claim[]
                {
                    new Claim(ClaimTypes.PrimarySid, id.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                //NotBefore = expires,
                Expires = expires.AddHours(6),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
