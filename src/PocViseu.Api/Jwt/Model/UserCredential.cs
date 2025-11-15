namespace PocViseu.Api.Jwt.Model
{
    public class UserCredential
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public long? conId { get; set; }
    }
}
