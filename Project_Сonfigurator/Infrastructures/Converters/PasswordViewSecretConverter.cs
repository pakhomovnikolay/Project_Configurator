using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Project_Сonfigurator.Infrastructures.Converters
{
    [MarkupExtensionReturnType(typeof(PasswordViewSecretConverter))]
    public class PasswordViewSecretConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not string password) return null;
            var output = "";
            for (int i = 0; i < password.Length; i++)
                output += "*";

            return output;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            if (v is not string password) return null;
            var output = "";
            for (int i = 0; i < password.Length; i++)
                output += "*";

            return output;

            //throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
