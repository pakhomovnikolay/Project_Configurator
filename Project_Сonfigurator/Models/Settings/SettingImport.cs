using Project_Сonfigurator.Models.Settings.Interfaces;

namespace Project_Сonfigurator.Models.Settings
{
    public class SettingImport : ISettingImport
    {
        #region Стартовый индекс строки импорта
        /// <summary>
        /// Стартовый индекс строки импорта
        /// </summary>
        public string StartIndexRow { get; set; }
        #endregion

        #region Индекс колонки "Идентификатор"
        /// <summary>
        /// Индекс колонки "Идентификатор"
        /// </summary>
        public string IndexColumnId { get; set; }
        #endregion

        #region Индекс колонки "Наименование сигнала"
        /// <summary>
        /// Индекс колонки "Наименование сигнала"
        /// </summary>
        public string IndexColumnDescription { get; set; }
        #endregion

        #region Индекс колонки "Номер корзины"
        /// <summary>
        /// Индекс колонки "Номер корзины"
        /// </summary>
        public string IndexColumnRack { get; set; }
        #endregion

        #region Индекс колонки "Номер канала в модуле"
        /// <summary>
        /// Индекс колонки "Номер канала в модуле"
        /// </summary>
        public string IndexColumnModule { get; set; }
        #endregion
    }
}
