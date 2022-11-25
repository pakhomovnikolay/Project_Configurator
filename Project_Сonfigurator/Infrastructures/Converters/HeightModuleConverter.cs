using Project_Сonfigurator.Infrastructures.Enum;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

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
        public int HeighRow
        {
            get => (int)GetValue(HeighRowProperty);
            set => SetValue(HeighRowProperty, value);
        }

        public static readonly DependencyProperty HeighRowProperty =
            DependencyProperty.Register(
                nameof(HeighRow),
                typeof(int),
                typeof(HeightModuleConverter),
                new PropertyMetadata(default(int)));
        #endregion

        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is null) return 0;
            var Value = v.ToString();

            #region Модули DI
            if (Value.Contains("DI", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("DAI", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("DDI", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("DDM", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("DRA", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("DRC", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("ERT", StringComparison.CurrentCultureIgnoreCase)
                    )
                return 25 * 32;
            #endregion

            #region Модули DO
            else if (Value.Contains("DO", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("DAO", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("DDO", StringComparison.CurrentCultureIgnoreCase)
                    )
                return 25 * 32;
            #endregion

            #region Модули AI
            else if (Value.Contains("AI", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("AHI", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("AMI", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("AMM", StringComparison.CurrentCultureIgnoreCase)
                    )
                return 25;
            #endregion

            #region Модули AO
            else if (Value.Contains("AO", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("AMO", StringComparison.CurrentCultureIgnoreCase) ||
                    Value.Contains("AHO", StringComparison.CurrentCultureIgnoreCase)
                    )
                return 25;
            #endregion

            else
            {
                var CountRow = v.ToString();
                var type = p is not null ? p.ToString() : "";
                return int.TryParse(CountRow, out int count_row) ? count_row * HeighRow : 0;
            }
            
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
