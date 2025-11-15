
namespace PocViseu.Api.Jwt.Model
{
    public class AuthenticationResponse
    {
        public long Id { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string? Fullname { get; set; }
    }
}
