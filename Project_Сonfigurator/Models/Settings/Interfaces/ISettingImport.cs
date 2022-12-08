namespace Project_Сonfigurator.Models.Settings.Interfaces
{
    public interface ISettingImport
    {
        #region Стартовый индекс строки импорта
        /// <summary>
        /// Стартовый индекс строки импорта
        /// </summary>
        string StartIndexRow { get; set; }
        #endregion

        #region Индекс колонки "Идентификатор"
        /// <summary>
        /// Индекс колонки "Идентификатор"
        /// </summary>
        string IndexColumnId { get; set; }
        #endregion

        #region Индекс колонки "Наименование сигнала"
        /// <summary>
        /// Индекс колонки "Наименование сигнала"
        /// </summary>
        string IndexColumnDescription { get; set; }
        #endregion

        #region Индекс колонки "Номер корзины"
        /// <summary>
        /// Индекс колонки "Номер корзины"
        /// </summary>
        string IndexColumnRack { get; set; }
        #endregion

        #region Индекс колонки "Номер канала в модуле"
        /// <summary>
        /// Индекс колонки "Номер канала в модуле"
        /// </summary>
        string IndexColumnModule { get; set; }
        #endregion
    }
}
