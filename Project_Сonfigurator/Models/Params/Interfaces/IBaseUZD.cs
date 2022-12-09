using Project_Сonfigurator.Models.Setpoints;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseUZD
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

        #region Наличие сигнала дистанционного ключа
        /// <summary>
        /// Наличие сигнала дистанционного ключа
        /// </summary>
        string Dist { get; set; }
        #endregion

        #region Наличие двух команд остановки
        /// <summary>
        /// Наличие двух команд остановки
        /// </summary>
        string DoubleStop { get; set; }
        #endregion

        #region Интелектуальный привод
        /// <summary>
        /// Интелектуальный привод
        /// </summary>
        string Bur { get; set; }
        #endregion

        #region Наличие сигнала контроля цепей открытия
        /// <summary>
        /// Наличие сигнала контроля цепей открытия
        /// </summary>
        string COz { get; set; }
        #endregion

        #region Наличие сигнала контроля цепей закрытия
        /// <summary>
        /// Наличие сигнала контроля цепей закрытия
        /// </summary>
        string CZz { get; set; }
        #endregion

        #region Наличие сигнала контроля напряжения
        /// <summary>
        /// Наличие сигнала контроля напряжения
        /// </summary>
        string EC { get; set; }
        #endregion

        #region Контроль подозрительных состояний
        /// <summary>
        /// Контроль подозрительных состояний
        /// </summary>
        string CheckState { get; set; }
        #endregion

        #region Отсутствие интерфейсного сигнала
        /// <summary>
        /// Отсутствие интерфейсного сигнала
        /// </summary>
        string RsOff { get; set; }
        #endregion

        #region Тип ЗД
        /// <summary>
        /// Тип ЗД
        /// </summary>
        string TypeZD { get; set; }
        #endregion

        #region Номер пожарной зоны
        /// <summary>
        /// Номер пожарной зоны
        /// </summary>
        string IndexPZ { get; set; }
        #endregion

        #region Номер бака дозатора
        /// <summary>
        /// Номер бака дозатора
        /// </summary>
        string IndexBD { get; set; }
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
