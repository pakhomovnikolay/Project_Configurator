using System.Collections.Generic;

namespace Template.Infrastructures.Validate
{
    public static class ValidationIP
    {
        public static string Check(string ip_address)
        {
            var ip_array = ip_address.Split('.');
            var ip_list = new List<string>();

            for (int i = 0; i < ip_array.Length; i++)
            {
                if (ip_list.Count > 3)
                    continue;

                ValidationNumeric.Check(ip_array[i], out var output, 3);
                ip_list.Add(output);
                if ((ip_array.Length - 1) < (i + 1))
                    if (ip_list.Count <= 3 && ip_array[i].Length > 3)
                    {
                        ValidationNumeric.Check(ip_array[i].Remove(0, ip_array[i].Length - 1), out output, 3);
                        ip_list.Add(output);
                    }
            }
            return string.Join(".", ip_list);
        }
    }
}
