using Project_Сonfigurator.Models.Setpoints;
using System.Collections.Generic;

namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseUMPNA
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

        #region Номер приемной задвижки
        /// <summary>
        /// Номер приемной задвижки
        /// </summary>
        string IndexPZ { get; set; }
        #endregion

        #region Номер выкидной задвижки
        /// <summary>
        /// Номер выкидной задвижки
        /// </summary>
        string IndexVZ { get; set; }
        #endregion

        #region Насос типа НМ
        /// <summary>
        /// Насос типа НМ
        /// </summary>
        string TypeUMPNA { get; set; }
        #endregion

        #region Номер группы маслоситемы (при индивидульной маслосистемы)
        /// <summary>
        /// Номер группы маслоситемы (при индивидульной маслосистемы)
        /// </summary>
        string IndexGroupMS { get; set; }
        #endregion

        #region Использовать ФБ "Ограничение кол-во пусков"
        /// <summary>
        /// Использовать ФБ "Ограничение кол-во пусков"
        /// </summary>
        string UsedMCP { get; set; }
        #endregion

        #region Использовать ФБ "Предпусковой продувки ЭД"
        /// <summary>
        /// Использовать ФБ "Предпусковой продувки ЭД"
        /// </summary>
        string UsedKPD { get; set; }
        #endregion

        #region Кол-во стоповых кнопок
        /// <summary>
        /// Кол-во стоповых кнопок
        /// </summary>
        string CountButtonStop { get; set; }
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
