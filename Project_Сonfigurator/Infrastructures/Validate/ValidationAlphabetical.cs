using System.Text.RegularExpressions;

namespace Template.Infrastructures.Validate
{
    public static class ValidationAlphabetical
    {
        public static string Check(string data)
        {
            var x = data.Length - 1;
            var pattern1 = @"^\w*$";
            var pattern2 = @"^\d*$";
            var output = data;
            if (Regex.IsMatch(data, pattern1, RegexOptions.IgnoreCase))
                if (Regex.IsMatch(data, pattern2, RegexOptions.IgnoreCase))
                    output = data.Remove(x);

            return output;

        }
    }
}
