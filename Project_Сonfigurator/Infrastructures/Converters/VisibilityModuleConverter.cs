using Project_Сonfigurator.Infrastructures.Enum;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Project_Сonfigurator.Infrastructures.Converters
{
    [MarkupExtensionReturnType(typeof(VisibilityModuleConverter))]
    public class VisibilityModuleConverter : Freezable, IValueConverter
    {
        protected override Freezable CreateInstanceCore() => new VisibilityModuleConverter();

        #region Видимость аналоговых модулей
        /// <summary>
        /// Видимость аналоговых модулей
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
                typeof(VisibilityModuleConverter),
                new PropertyMetadata(default(int)));
        #endregion

        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not TypeModule type) return Visibility.Collapsed;
            Visibility visible = Visibility.Collapsed;
            switch (type)
            {
                case TypeModule.AI:
                case TypeModule.AO:
                    if (CountRow == 8)
                        visible = Visibility.Visible;
                    break;
                case TypeModule.DI:
                case TypeModule.DO:
                    if (CountRow == 32)
                        visible = Visibility.Visible;
                    break;
            }

            return visible;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
