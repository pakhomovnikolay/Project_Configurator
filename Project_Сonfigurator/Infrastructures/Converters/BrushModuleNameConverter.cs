using System.Globalization;
using System;
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
            if (v is not string brush) return null;


            #region Заглушки\Оконечные модуля
            if (brush.Contains("ST", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("XEM", StringComparison.CurrentCultureIgnoreCase))
                return "#FFD9D9D9";
            #endregion

            #region Интерфейсные модуля
            else if ((brush.Contains("CP", StringComparison.CurrentCultureIgnoreCase) && !brush.Contains("CPS", StringComparison.CurrentCultureIgnoreCase)) ||
                    brush.Contains("CXM", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("NOC", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("NOP", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("NOR", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("NOS", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("EIA", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("NGD", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("NOM", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("NRP", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("CRA", StringComparison.CurrentCultureIgnoreCase)
                    )
                return "#FFED7D31";
            #endregion

            #region Модули DI
            else if (brush.Contains("DI", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("DAI", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("DDI", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("DDM", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("DRA", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("DRC", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("ERT", StringComparison.CurrentCultureIgnoreCase)
                    )
                return "#FFFFC000";
            #endregion

            #region Модули AI
            else if (brush.Contains("AI", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("AHI", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("AMI", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("AMM", StringComparison.CurrentCultureIgnoreCase)
                    )
                return "#FF70AD47";
            #endregion

            #region Модули DO
            else if (brush.Contains("DO", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("DAO", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("DDO", StringComparison.CurrentCultureIgnoreCase)
                    )
                return "#FF00B0F0";
            #endregion

            #region Модули AO
            else if (brush.Contains("AO", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("AMO", StringComparison.CurrentCultureIgnoreCase) ||
                    brush.Contains("AHO", StringComparison.CurrentCultureIgnoreCase)
                    )
                return "#FF7030A0";
            #endregion

            #region Счетные модули
            else if (brush.Contains("DA", StringComparison.CurrentCultureIgnoreCase) ||
                brush.Contains("ART", StringComparison.CurrentCultureIgnoreCase))
                return "#FF45F4FD";
            #endregion

            #region ПЛК
            else if (brush.Contains("Safety", StringComparison.CurrentCultureIgnoreCase))
                return Brushes.Yellow;

            #endregion

            else
                return Brushes.White;

        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
