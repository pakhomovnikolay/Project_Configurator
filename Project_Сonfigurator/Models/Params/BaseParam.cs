﻿using Project_Сonfigurator.Models.Params.Interfaces;
using System.Windows;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseParam : Freezable, IBaseParam
    {
        protected override Freezable CreateInstanceCore() => new BaseParam();

        #region Индекс параметра
        /// <summary>
        /// Индекс параметра
        /// </summary>
        public string Index { get; set; }
        #endregion

        #region Идентификатор
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id
        {
            get => (string)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register(
                nameof(Id),
                typeof(string),
                typeof(BaseParam),
                new PropertyMetadata(default(string)));
        #endregion

        #region Описание параметра
        /// <summary>
        /// Описание параметра
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Имя переменной
        /// <summary>
        /// Имя переменной
        /// </summary>
        public string VarName { get; set; }
        #endregion

        #region Инверсия
        private string _Inv;
        /// <summary>
        /// Инверсия
        /// </summary>
        public string Inv
        {
            get { return _Inv; }
            set
            {
                _Inv = value;
                if (string.IsNullOrWhiteSpace(_Inv)) return;
                if (!int.TryParse(_Inv, out int _value))
                    _Inv = _value.ToString();
                else if (_value > 1) _Inv = "1";
            }
        }
        #endregion

        #region Тип сигнала
        /// <summary>
        /// Тип сигнала
        /// </summary>
        public string TypeSignal { get; set; }
        #endregion

        #region Смещенеие
        private string _Address;
        /// <summary>
        /// Смещенеие
        /// </summary>
        public string Address
        {
            get => _Address;
            set
            {
                _Address = value;
                if (_Address == null || string.IsNullOrWhiteSpace(_Address))
                    Id = "";
            }
        }
        #endregion
    }
}
