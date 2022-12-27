using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Project_Сonfigurator.Infrastructures.Converters
{
    [MarkupExtensionReturnType(typeof(BrushConverter))]
    public class BrushConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not string) return null;

            Brush brush = v switch
            {

                "Красный" => Brushes.Red,
                "Желтый" => Brushes.Yellow,
                "Зеленый" => Brushes.Green,
                _ => Brushes.Transparent,
            };

            return brush;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
