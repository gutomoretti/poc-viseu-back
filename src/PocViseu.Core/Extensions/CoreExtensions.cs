using Newtonsoft.Json;
using PocViseu.Core.Crypt;

namespace PocViseu.Core.Extensions
{
    public static class CoreExtensions
    {

        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.None,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });
        }

        public static string ToHash(this string value)
        {
            return WebControlCrypt.HashPassword(value);
        }

        public static bool ToVerify(this string value, string hashPassword)
        {
            return WebControlCrypt.Verify(value, hashPassword);
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

    }
}
