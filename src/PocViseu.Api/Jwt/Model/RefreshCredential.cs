namespace PocViseu.Api.Jwt.Model
{
    public class RefreshCredential
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
