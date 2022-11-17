using System.Text.RegularExpressions;

namespace Template.Infrastructures.Validate
{
    public static class ValidationNumeric
    {
        public static bool Check(string data, out string output, int CountDigital = 0)
        {
            output = data;
            var pattern = @"^\d*$";
            var IsMatch = Regex.IsMatch(data, pattern, RegexOptions.IgnoreCase);

            if (!IsMatch)
            {
                output = "0";
                return false;
            }
            else if ((CountDigital > 0) && (data.Length > CountDigital))
            {
                output = data.Remove(data.Length - 1);
                return false;
            }

            return true;
        }
    }
}
