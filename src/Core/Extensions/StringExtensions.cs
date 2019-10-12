namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static string MustEndWith(this string str, string end)
        {
            if (str.EndsWith(end))
                return str;

            return str + end;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return str == null || str.Trim().Length <= 0;
        }

        public static string Format(this string str, params object[] args)
        {
            return string.Format(str, args);
        }
    }
}