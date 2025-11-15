using System.Security.Claims;
using PocViseu.Api.Jwt.Model;
using PocViseu.Model.Auth;

namespace PocViseu.Api.Jwt.Interfaces
{
    public interface IJWTAuthenticationManager
    {
        AuthenticationResponse Authenticate(User user);
        IDictionary<string, string> UsersRefreshTokens { get; set; }
        AuthenticationResponse Authenticate(string username, Claim[] claims);
    }
}
