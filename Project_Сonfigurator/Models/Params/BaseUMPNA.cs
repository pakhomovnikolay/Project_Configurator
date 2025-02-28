﻿using Project_Сonfigurator.Models.Params.Interfaces;
using Project_Сonfigurator.Models.Setpoints;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseUMPNA : IBaseUMPNA
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

        #region Номер приемной задвижки
        /// <summary>
        /// Номер приемной задвижки
        /// </summary>
        public string IndexPZ { get; set; }
        #endregion

        #region Номер выкидной задвижки
        /// <summary>
        /// Номер выкидной задвижки
        /// </summary>
        public string IndexVZ { get; set; }
        #endregion

        #region Насос типа НМ
        /// <summary>
        /// Насос типа НМ
        /// </summary>
        public string TypeUMPNA { get; set; }
        #endregion

        #region Номер группы маслоситемы (при индивидульной маслосистемы)
        /// <summary>
        /// Номер группы маслоситемы (при индивидульной маслосистемы)
        /// </summary>
        public string IndexGroupMS { get; set; }
        #endregion

        #region Использовать ФБ "Ограничение кол-во пусков"
        /// <summary>
        /// Использовать ФБ "Ограничение кол-во пусков"
        /// </summary>
        public string UsedMCP { get; set; }
        #endregion

        #region Использовать ФБ "Предпусковой продувки ЭД"
        /// <summary>
        /// Использовать ФБ "Предпусковой продувки ЭД"
        /// </summary>
        public string UsedKPD { get; set; }
        #endregion

        #region Кол-во стоповых кнопок
        /// <summary>
        /// Кол-во стоповых кнопок
        /// </summary>
        public string CountButtonStop { get; set; }
        #endregion

        #region Входные параметры
        /// <summary>
        /// Входные параметры
        /// </summary>
        public ObservableCollection<BaseParam> InputParam { get; set; } = new();
        #endregion

        #region Выходные параметры
        /// <summary>
        /// Выходные параметры
        /// </summary>
        public ObservableCollection<BaseParam> OutputParam { get; set; } = new();
        #endregion

        #region Временные уставки
        /// <summary>
        /// Временные уставки
        /// </summary>
        public ObservableCollection<BaseSetpoints> Setpoints { get; set; } = new();
        #endregion

        #region Параметры готовностей
        /// <summary>
        /// Параметры готовностей
        /// </summary>
        public ObservableCollection<BaseKGMPNA> KGMPNA { get; set; } = new();
        #endregion

        #region Параметры защит
        /// <summary>
        /// Параметры защит
        /// </summary>
        public ObservableCollection<BaseKTPRA> KTPRA { get; set; } = new();
        #endregion

        #region Параметры предельных параметров
        /// <summary>
        /// Параметры предельных параметров
        /// </summary>
        public ObservableCollection<BaseKTPRAS> KTPRAS { get; set; } = new();
        #endregion
    }
}
