namespace WebApi.Extensions
{
    public static class StringExtensions
    {
        public static bool IsStringNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }
    }
}