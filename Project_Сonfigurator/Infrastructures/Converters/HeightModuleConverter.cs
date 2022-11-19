using System.Globalization;
using System;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows;

namespace Project_Сonfigurator.Infrastructures.Converters
{
    [MarkupExtensionReturnType(typeof(HeightModuleConverter))]
    public class HeightModuleConverter : Freezable, IValueConverter
    {
        protected override Freezable CreateInstanceCore() => new HeightModuleConverter();

        #region Количество строк
        /// <summary>
        /// Количество строк
        /// </summary>
        public int CountRow
        {
            get => (int)GetValue(CountRowProperty);
            set => SetValue(CountRowProperty, value);
        }

        public static readonly DependencyProperty CountRowProperty =
            DependencyProperty.Register(
                nameof(CountRow),
                typeof(int),
                typeof(HeightModuleConverter),
                new PropertyMetadata(default(int)));
        #endregion

        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is null) return 0;
            var HeighRow = (string)v;
            return int.TryParse(HeighRow, out int result_heigh_row) ? result_heigh_row * CountRow : 0;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
