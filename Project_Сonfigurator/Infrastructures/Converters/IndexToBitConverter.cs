using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Project_Сonfigurator.Infrastructures.Converters
{
    [MarkupExtensionReturnType(typeof(IndexToBitConverter))]
    public class IndexToBitConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not string index) return null;
            if (string.IsNullOrWhiteSpace(index)) return null;
            var Index = int.Parse(index) - 1;
            return Index % 16;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
