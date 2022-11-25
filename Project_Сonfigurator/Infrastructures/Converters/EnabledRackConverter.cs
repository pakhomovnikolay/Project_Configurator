﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Project_Сonfigurator.Infrastructures.Converters
{
    [MarkupExtensionReturnType(typeof(EnabledRackConverter))]
    public class EnabledRackConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not bool is_enable) return null;
            return is_enable ? Brushes.Black : Brushes.DarkGray;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
