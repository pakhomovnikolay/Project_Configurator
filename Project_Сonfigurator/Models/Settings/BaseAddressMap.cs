using Project_Сonfigurator.Models.Settings.Interfaces;
using System.Windows;

namespace Project_Сonfigurator.Models.Settings
{
    public class BaseAddressMap : Freezable, IBaseAddressMap
    {
        protected override Freezable CreateInstanceCore() => new BaseAddressMap();

        #region Индекс параметра
        /// <summary>
        /// Индекс параметра
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Описание переменной
        /// <summary>
        /// Описание переменной
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Путь к переменной
        /// <summary>
        /// Путь к переменной
        /// </summary>
        public string PathTag { get; set; }
        #endregion

        #region Длина (WORD)
        private string _LengthWord;
        /// <summary>
        /// Длина (WORD)
        /// </summary>
        public string LengthWord
        {
            get => _LengthWord;
            set
            {
                if (_LengthWord != value)
                {
                    _LengthWord = value;
                    _ = int.TryParse(_LengthWord, out int _Lengt);
                    _ = int.TryParse(AddressStart, out int _Address);
                    LengthByte = $"{_Lengt * 2}";
                    AddressEnd = $"{_Lengt + _Address - 1}";
                }
            }
        }
        #endregion

        #region Длина (BYTE)
        /// <summary>
        /// Длина (BYTE)
        /// </summary>
        public string LengthByte
        {
            get => (string)GetValue(LengthByteProperty);
            set => SetValue(LengthByteProperty, value);
        }

        public static readonly DependencyProperty LengthByteProperty =
            DependencyProperty.Register(
                nameof(LengthByte),
                typeof(string),
                typeof(BaseAddressMap),
                new PropertyMetadata(default(string)));
        #endregion

        #region Стартовый адрес
        /// <summary>
        /// Стартовый адрес
        /// </summary>
        public string AddressStart
        {
            get => (string)GetValue(AddressStartProperty);
            set => SetValue(AddressStartProperty, value);
        }

        public static readonly DependencyProperty AddressStartProperty =
            DependencyProperty.Register(
                nameof(AddressStart),
                typeof(string),
                typeof(BaseAddressMap),
                new PropertyMetadata(default(string)));
        #endregion

        #region Конечный адрес
        /// <summary>
        /// Конечный адрес
        /// </summary>
        public string AddressEnd
        {
            get => (string)GetValue(AddressEndProperty);
            set => SetValue(AddressEndProperty, value);
        }

        public static readonly DependencyProperty AddressEndProperty =
            DependencyProperty.Register(
                nameof(AddressEnd),
                typeof(string),
                typeof(BaseAddressMap),
                new PropertyMetadata(default(string)));
        #endregion

        #region Адрес в ПЛК
        /// <summary>
        /// Адрес в ПЛК
        /// </summary>
        public string AddressInPLC
        {
            get => (string)GetValue(AddressInPLCProperty);
            set => SetValue(AddressInPLCProperty, value);
        }

        public static readonly DependencyProperty AddressInPLCProperty =
            DependencyProperty.Register(
                nameof(AddressInPLC),
                typeof(string),
                typeof(BaseAddressMap),
                new PropertyMetadata(default(string)));
        #endregion
    }
}
