namespace Project_Сonfigurator.Models.Signals.Interfaces
{
    public interface IBaseSignal
    {
        #region Индекс в массиве
        /// <summary>
        /// Индекс в массиве
        /// </summary>
        string Index { get; set; }
        #endregion

        #region Идентификатор
        /// <summary>
        /// Идентификатор
        /// </summary>
        string Id { get; set; }
        #endregion

        #region Описание сигнала
        /// <summary>
        ///  Описание сигнала
        /// </summary>
        string Description { get; set; }
        #endregion

        #region Имя переменной
        /// <summary>
        /// Имя переменной
        /// </summary>
        string VarName { get; set; }
        #endregion

        #region Область
        /// <summary>
        /// Область (физический\формируемый сигнал) 
        /// </summary>
        string Area { get; set; }
        #endregion

        #region Смещение
        /// <summary>
        /// Смещение. Ссылка на сигнал
        /// </summary>
        string Address { get; set; }
        #endregion

        #region Значание ссылки
        /// <summary>
        /// Значание ссылки. Формируется с учетом области
        /// </summary>
        string LinkValue { get; set; }
        #endregion
    }
}
