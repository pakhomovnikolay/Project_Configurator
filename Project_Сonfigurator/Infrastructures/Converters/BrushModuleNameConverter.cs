using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Project_Сonfigurator.Infrastructures.Converters
{
    [MarkupExtensionReturnType(typeof(BrushModuleNameConverter))]
    public class BrushModuleNameConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not string DescriptionModule) return Brushes.White;

            #region Заглушки\Оконечные модуля
            if (DescriptionModule.Contains("ST", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("XEM", StringComparison.CurrentCultureIgnoreCase))
                return "#FFD9D9D9";
            #endregion

            #region Интерфейсные модуля
            else if ((DescriptionModule.Contains("CP", StringComparison.CurrentCultureIgnoreCase) && !DescriptionModule.Contains("CPS", StringComparison.CurrentCultureIgnoreCase)) ||
                    DescriptionModule.Contains("CXM", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("NOC", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("NOP", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("NOR", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("NOS", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("EIA", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("NGD", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("NOM", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("NRP", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("CRA", StringComparison.CurrentCultureIgnoreCase)
                    )
                return "#FFED7D31";
            #endregion

            #region Модули DI
            else if (DescriptionModule.Contains("DI", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("DAI", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("DDI", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("DDM", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("DRA", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("DRC", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("ERT", StringComparison.CurrentCultureIgnoreCase)
                    )
                return "#FFFFC000";
            #endregion

            #region Модули AI
            else if (DescriptionModule.Contains("AI", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("AHI", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("AMI", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("AMM", StringComparison.CurrentCultureIgnoreCase)
                    )
                return "#FF70AD47";
            #endregion

            #region Модули DO
            else if (DescriptionModule.Contains("DO", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("DAO", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("DDO", StringComparison.CurrentCultureIgnoreCase)
                    )
                return "#FF00B0F0";
            #endregion

            #region Модули AO
            else if (DescriptionModule.Contains("AO", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("AMO", StringComparison.CurrentCultureIgnoreCase) ||
                    DescriptionModule.Contains("AHO", StringComparison.CurrentCultureIgnoreCase)
                    )
                return "#FF7030A0";
            #endregion

            #region Счетные модули
            else if (DescriptionModule.Contains("DA", StringComparison.CurrentCultureIgnoreCase) ||
                DescriptionModule.Contains("ART", StringComparison.CurrentCultureIgnoreCase))
                return "#FF45F4FD";
            #endregion

            #region ПЛК
            else if (DescriptionModule.Contains("Safety", StringComparison.CurrentCultureIgnoreCase))
                return Brushes.Yellow;

            #endregion

            else if (!string.IsNullOrWhiteSpace(DescriptionModule))
                return Brushes.White;
            else
                return Brushes.Transparent;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
