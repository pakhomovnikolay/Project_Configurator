using Project_Сonfigurator.Models.Params.Interfaces;
using Project_Сonfigurator.Models.Setpoints;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Models.Params
{
    public class BaseUZD : IBaseUZD
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

        #region Наличие сигнала дистанционного ключа
        /// <summary>
        /// Наличие сигнала дистанционного ключа
        /// </summary>
        public string Dist { get; set; }
        #endregion

        #region Наличие двух команд остановки
        /// <summary>
        /// Наличие двух команд остановки
        /// </summary>
        public string DoubleStop { get; set; }
        #endregion

        #region Интелектуальный привод
        /// <summary>
        /// Интелектуальный привод
        /// </summary>
        public string Bur { get; set; }
        #endregion

        #region Наличие сигнала контроля цепей открытия
        /// <summary>
        /// Наличие сигнала контроля цепей открытия
        /// </summary>
        public string COz { get; set; }
        #endregion

        #region Наличие сигнала контроля цепей закрытия
        /// <summary>
        /// Наличие сигнала контроля цепей закрытия
        /// </summary>
        public string CZz { get; set; }
        #endregion

        #region Наличие сигнала контроля напряжения
        /// <summary>
        /// Наличие сигнала контроля напряжения
        /// </summary>
        public string EC { get; set; }
        #endregion

        #region Контроль подозрительных состояний
        /// <summary>
        /// Контроль подозрительных состояний
        /// </summary>
        public string CheckState { get; set; }
        #endregion

        #region Отсутствие интерфейсного сигнала
        /// <summary>
        /// Отсутствие интерфейсного сигнала
        /// </summary>
        public string RsOff { get; set; }
        #endregion

        #region Тип ЗД
        /// <summary>
        /// Тип ЗД
        /// </summary>
        public string TypeZD { get; set; }
        #endregion

        #region Номер пожарной зоны
        /// <summary>
        /// Номер пожарной зоны
        /// </summary>
        public string IndexPZ { get; set; }
        #endregion

        #region Номер базы данных
        /// <summary>
        /// Номер базы данных
        /// </summary>
        public string IndexBD { get; set; }
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
    }
}
