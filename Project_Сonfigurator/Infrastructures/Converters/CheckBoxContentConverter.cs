using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Project_Сonfigurator.Infrastructures.Converters
{
    [MarkupExtensionReturnType(typeof(CheckBoxContentConverter))]
    public class CheckBoxContentConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not bool IsChecked) { return "Выбрать все"; }
            string Text;
            if (!IsChecked) { Text = "Выбрать все"; }
            else { Text = "Снять все"; }
            return Text;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
