using Project_Сonfigurator.Models.Setpoints;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseUVS
    {
        #region Индекс параметра
        /// <summary>
        /// Индекс параметра
        /// </summary>
        string Index { get; set; }
        #endregion

        #region Описание параметра
        /// <summary>
        /// Описание параметра
        /// </summary>
        string Description { get; set; }
        #endregion

        #region Имя переменной
        /// <summary>
        /// Имя переменной
        /// </summary>
        string VarName { get; set; }
        #endregion

        #region Короткое обозначение
        /// <summary>
        /// Короткое обозначение
        /// </summary>
        string ShortDescription { get; set; }
        #endregion

        #region Номер секции шин
        /// <summary>
        /// Номер секции шин
        /// </summary>
        string IndexEC { get; set; }
        #endregion

        #region Номер группы
        /// <summary>
        /// Номер группы
        /// </summary>
        string IndexGroup { get; set; }
        #endregion

        #region Описание группы
        /// <summary>
        /// Описание группы
        /// </summary>
        string DescriptionGroup { get; set; }
        #endregion

        #region Наличие резервного насоса
        /// <summary>
        /// Наличие резервного насоса
        /// </summary>
        string Reservable { get; set; }
        #endregion

        #region Тип датчика давления
        /// <summary>
        /// Тип датчика давления
        /// </summary>
        string TypePressure { get; set; }
        #endregion

        #region Наличие контроля цепей включения
        /// <summary>
        /// Наличие контроля цепей включения
        /// </summary>
        string COz { get; set; }
        #endregion

        #region Один датчик давления на группу
        /// <summary>
        /// Один датчик давления на группу
        /// </summary>
        string OnPressureSensorGroup { get; set; }
        #endregion

        #region Тип группы вспомсистем
        /// <summary>
        /// Тип группы вспомсистем
        /// </summary>
        string TypeGroup { get; set; }
        #endregion

        #region Входные параметры
        /// <summary>
        /// Входные параметры
        /// </summary>
        List<BaseParam> InputParam { get; set; }
        #endregion

        #region Выходные параметры
        /// <summary>
        /// Выходные параметры
        /// </summary>
        List<BaseParam> OutputParam { get; set; }
        #endregion

        #region Временные уставки
        /// <summary>
        /// Временные уставки
        /// </summary>
        List<BaseSetpoints> Setpoints { get; set; }
        #endregion
    }
}
