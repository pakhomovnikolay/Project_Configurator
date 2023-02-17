using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Project_Сonfigurator.Infrastructures.Converters.UserDIUserControl
{
    [MarkupExtensionReturnType(typeof(IndexToBorderThicknessConverter))]
    public class IndexToBorderThicknessConverter : Freezable, IValueConverter
    {
        protected override Freezable CreateInstanceCore() => new IndexToBorderThicknessConverter();

        #region Индекс колонки
        /// <summary>
        /// Индекс колонки
        /// </summary>
        public int IndexColumn
        {
            get => (int)GetValue(IndexColumnProperty);
            set => SetValue(IndexColumnProperty, value);
        }

        public static readonly DependencyProperty IndexColumnProperty =
            DependencyProperty.Register(
                nameof(IndexColumn),
                typeof(int),
                typeof(IndexToBorderThicknessConverter),
                new PropertyMetadata(default(int)));
        #endregion

        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not string index) return null;
            if (string.IsNullOrWhiteSpace(index)) return null;
            var Index = int.Parse(index) - 1;
            var Bit = Index % 16;

            var _Thickness = IndexColumn switch
            {
                0 => Bit == 15 ? new Thickness(0, 0, 1, 2) : new Thickness(0, 0, 1, 1),
                3 => Bit == 15 ? new Thickness(0, 0, 1, 2) : new Thickness(0, 0, 1, 0),
                5 => Bit == 15 ? new Thickness(0, 0, 0, 2) : new Thickness(0, 0, 0, 1),

                _ => Bit == 15 ? new Thickness(0, 0, 1, 2) : new Thickness(0, 0, 1, 1),
            };

            return _Thickness;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
