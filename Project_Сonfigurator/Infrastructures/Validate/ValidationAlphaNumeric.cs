using System.Text.RegularExpressions;

namespace Template.Infrastructures.Validate
{
    public static class ValidationAlphaNumeric
    {
        public static string Check(string data)
        {
            var x = data.Length - 1;
            var pattern = @"^\w*$";
            if (Regex.IsMatch(data, pattern, RegexOptions.IgnoreCase))
                return data;
            else
                return data.Remove(x);
        }
    }
}
