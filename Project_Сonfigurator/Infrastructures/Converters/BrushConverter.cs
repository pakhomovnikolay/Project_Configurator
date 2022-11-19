using System.Globalization;
using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Markup;

namespace Project_Сonfigurator.Infrastructures.Converters
{
    [MarkupExtensionReturnType(typeof(BrushConverter))]
    public class BrushConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not Color brush) return null;
            return new SolidColorBrush(brush);
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
