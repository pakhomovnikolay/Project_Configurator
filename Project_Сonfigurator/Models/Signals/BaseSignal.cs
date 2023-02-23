using Project_Сonfigurator.Models.Signals.Interfaces;
using System.Windows;

namespace Project_Сonfigurator.Models.Signals
{
    public class BaseSignal : Freezable, IBaseSignal
    {
        protected override Freezable CreateInstanceCore() => new BaseSignal();

        #region Индекс в массиве
        /// <summary>
        /// Индекс в массиве
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Идентификатор
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }
        #endregion

        #region Описание сигнала
        /// <summary>
        ///  Описание сигнала
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Имя переменной
        /// <summary>
        /// Имя переменной
        /// </summary>
        public string VarName { get; set; }
        #endregion

        #region Область
        private string _Area;
        /// <summary>
        /// Область (физический\формируемый сигнал) 
        /// </summary>
        public string Area
        {
            get { return _Area; }
            set
            {
                _Area = value;
                
                LinkValue = "";
                var address = 0;
                var area = 0;
                if (!string.IsNullOrWhiteSpace(Address))
                    address = int.Parse(Address);

                if (!string.IsNullOrWhiteSpace(_Area))
                {
                    _ = int.TryParse(_Area, out area);
                    if (area > 1) area = 1;
                    _Area = area.ToString();
                }

                if (address > 0 || area > 0)
                {
                    LinkValue = $"{area * 32768 + address}";
                }
            }
        }
        #endregion

        #region Смещение
        private string _Address;
        /// <summary>
        /// Смещение. Ссылка на сигнал
        /// </summary>
        public string Address
        {
            get { return _Address; }
            set
            {
                _Address = value;
                LinkValue = "";
                var address = 0;
                var area = 0;
                if (!string.IsNullOrWhiteSpace(Address))
                    int.TryParse(Address, out address);

                if (!string.IsNullOrWhiteSpace(_Area))
                {
                    area = int.Parse(_Area);
                    if (area > 0)
                    {
                        area = 1;
                        _Area = "1";
                    }
                }

                LinkValue = $"{area * 32768 + address}";
            }
        }
        #endregion

        #region Значание ссылки
        /// <summary>
        /// Значание ссылки
        /// </summary>
        public string LinkValue
        {
            get => (string)GetValue(LinkValueProperty);
            set => SetValue(LinkValueProperty, value);
        }

        public static readonly DependencyProperty LinkValueProperty =
            DependencyProperty.Register(
                nameof(LinkValue),
                typeof(string),
                typeof(BaseSignal),
                new PropertyMetadata(default(string)));
        #endregion
    }
}
