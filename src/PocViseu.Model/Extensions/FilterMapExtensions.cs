namespace PocViseu.Model.Extensions
{
    public static class FilterMapExtensions
    {

        public static bool SearchId(this string? q, string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return true;
            }

            return q == null ? true : param.Equals(q);
        }

        public static bool Search(this string q, string param)
        {
            if (string.IsNullOrEmpty(param))
                return true;

            return q == null ? true : param.ToLower().Contains(q.ToLower());
        }

        public static bool SearchTraceKey(this string q, string param)
        {
            if (string.IsNullOrEmpty(param))
                return false;

            return q == null ? true : param.ToLower().Contains(q.ToLower());
        }

        public static string PrepareRut(this string value)
        {
            return value.Replace("-", "").Replace(".", "");
        }

        public static bool SearchRut(this string q, string param)
        {
            return q == null ? true : param.ToLower().PrepareRut().Contains(q.ToLower().PrepareRut());
        }

        public static bool SearchEqual(this string q, string param)
        {
            return q == null ? true : param.ToLower().Equals(q.ToLower());
        }

        public static bool Search(this int? q, int? param)
        {
            return q == null ? true : param == q;
        }

        public static bool Search(this long? q, long? param)
        {
            return q == null ? true : param == q;
        }

        public static bool SearchBool(this bool? q, bool? param)
        {
            return q == null ? true : param == q;
        }

        public static bool SearchData(this DateTime? q, DateTime? param)
        {
            return q == null ? true : param == q;
        }

        public static bool SearchDataBetween(this DateTime? param, DateTime? q, DateTime? param2)
        {
            if (q == null)
                return true;
            if (param2 == null)
                return true;
            if (param == null)
                return true;


            if (q >= param)
                if (q <= param2)
                    return true;

            return false;
        }
    }
}
