namespace Project_Сonfigurator.Models.Params.Interfaces
{
    public interface IBaseParam
    {
        #region Индекс параметра
        /// <summary>
        /// Индекс параметра
        /// </summary>
        string Index { get; set; }
        #endregion

        #region Идентификатор
        /// <summary>
        /// Идентификатор
        /// </summary>
        string Id { get; set; }
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

        #region Инверсия
        /// <summary>
        /// Инверсия
        /// </summary>
        string Inv { get; set; }
        #endregion

        #region Тип сигнала
        /// <summary>
        /// Тип сигнала
        /// </summary>
        string TypeSignal { get; set; }
        #endregion

        #region Смещенеие
        /// <summary>
        /// Смещенеие
        /// </summary>
        string Address { get; set; }
        #endregion
    }
}