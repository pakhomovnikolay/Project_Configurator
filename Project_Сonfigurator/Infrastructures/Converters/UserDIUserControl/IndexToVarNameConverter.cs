using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Project_Сonfigurator.Infrastructures.Converters.UserDIUserControl
{
    [MarkupExtensionReturnType(typeof(IndexToVarNameConverter))]
    public class IndexToVarNameConverter : Freezable, IValueConverter
    {
        protected override Freezable CreateInstanceCore() => new IndexToVarNameConverter();

        #region Имя переменной
        /// <summary>
        /// Имя переменной
        /// </summary>
        public string VarName
        {
            get => (string)GetValue(VarNameProperty);
            set => SetValue(VarNameProperty, value);
        }

        public static readonly DependencyProperty VarNameProperty =
            DependencyProperty.Register(
                nameof(VarName),
                typeof(string),
                typeof(IndexToVarNameConverter),
                new PropertyMetadata(default(string)));
        #endregion

        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not string index) return null;
            if (string.IsNullOrWhiteSpace(index)) return null;
            var Index = int.Parse(index) - 1;
            var Bit = Index % 16;

            var var_name = "";
            if (Bit == 7)
                var_name = VarName;

            return var_name;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotImplementedException("Обратное преобразование не поддерживается");
        }
    }
}
