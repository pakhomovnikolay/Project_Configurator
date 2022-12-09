using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Project_Сonfigurator.Infrastructures.Converters
{
    [MarkupExtensionReturnType(typeof(FilteredBackLightConverter))]
    public class FilteredBackLightConverter : Freezable, IMultiValueConverter
    {
        protected override Freezable CreateInstanceCore() => new FilteredBackLightConverter();

        #region Текст фильтра
        /// <summary>
        /// Текст фильтра
        /// </summary>
        public string TextFilter
        {
            get => (string)GetValue(TextFilterProperty);
            set => SetValue(TextFilterProperty, value);
        }

        public static readonly DependencyProperty TextFilterProperty =
            DependencyProperty.Register(
                nameof(TextFilter),
                typeof(string),
                typeof(FilteredBackLightConverter),
                new PropertyMetadata(default(string)));
        #endregion

        public object Convert(object[] v, Type t, object p, CultureInfo c)
        {
            if (v is null) return Brushes.White;
            if (string.IsNullOrWhiteSpace(TextFilter)) return Brushes.White;
            foreach (var text in v)
            {
                if (text is not string _text) return Brushes.White;
                if (TextFilter == _text) return Brushes.AliceBlue;
            }
            return Brushes.White;
        }

        public object[] ConvertBack(object v, Type[] t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
