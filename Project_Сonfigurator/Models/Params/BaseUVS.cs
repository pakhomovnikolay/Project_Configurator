using Project_Сonfigurator.Models.Params.Interfaces;
using Project_Сonfigurator.Models.Setpoints;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseUVS : IBaseUVS
    {
        #region Индекс параметра
        /// <summary>
        /// Индекс параметра
        /// </summary>
        public string Index { get; set; }
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

        #region Короткое обозначение
        /// <summary>
        /// Короткое обозначение
        /// </summary>
        public string ShortDescription { get; set; }
        #endregion

        #region Номер секции шин
        /// <summary>
        /// Номер секции шин
        /// </summary>
        public string IndexEC { get; set; }
        #endregion

        #region Номер группы
        /// <summary>
        /// Номер группы
        /// </summary>
        public string IndexGroup { get; set; }
        #endregion

        #region Описание группы
        /// <summary>
        /// Описание группы
        /// </summary>
        public string DescriptionGroup { get; set; }
        #endregion

        #region Наличие резервного насоса
        /// <summary>
        /// Наличие резервного насоса
        /// </summary>
        public string Reservable { get; set; }
        #endregion

        #region Тип датчика давления
        /// <summary>
        /// Тип датчика давления
        /// </summary>
        public string TypePressure { get; set; }
        #endregion

        #region Наличие контроля цепей включения
        /// <summary>
        /// Наличие контроля цепей включения
        /// </summary>
        public string COz { get; set; }
        #endregion

        #region Один датчик давления на группу
        /// <summary>
        /// Один датчик давления на группу
        /// </summary>
        public string OnePressureSensorGroup { get; set; }
        #endregion

        #region Тип группы вспомсистем
        /// <summary>
        /// Тип группы вспомсистем
        /// </summary>
        public string TypeGroup { get; set; }
        #endregion

        #region Входные параметры
        /// <summary>
        /// Входные параметры
        /// </summary>
        public List<BaseParam> InputParam { get; set; }
        #endregion

        #region Выходные параметры
        /// <summary>
        /// Выходные параметры
        /// </summary>
        public List<BaseParam> OutputParam { get; set; }
        #endregion

        #region Временные уставки
        /// <summary>
        /// Временные уставки
        /// </summary>
        public List<BaseSetpoints> Setpoints { get; set; }
        #endregion
    }
}
