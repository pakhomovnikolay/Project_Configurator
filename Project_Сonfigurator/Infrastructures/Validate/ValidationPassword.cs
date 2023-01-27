namespace Project_Сonfigurator.Infrastructures.Validate
{
    public static class ValidationPassword
    {
        public static string Check(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return "";
            var output = "";
            for (int i = 0; i < data.Length; i++)
                output += "*";

            return output;
        }
    }
}
