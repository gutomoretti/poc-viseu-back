namespace PocViseu.Core.Crypt
{
    public static class WebControlCrypt
    {
        public static string HashPassword(string value)
        {
            return BCrypt.Net.BCrypt.HashPassword(value);
        }

        public static bool Verify(string value, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(value, passwordHash);
        }
    }
}
