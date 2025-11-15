
using PocViseu.Api.Jwt.Model;

namespace PocViseu.Api.Jwt.Interfaces
{
    public interface ITokenRefresher
    {
        AuthenticationResponse Refresh(RefreshCredential refreshCred);
    }
}
