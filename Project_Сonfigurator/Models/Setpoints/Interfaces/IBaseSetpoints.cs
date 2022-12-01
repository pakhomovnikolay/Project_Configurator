namespace Project_Сonfigurator.Models.Setpoints.Interfaces
{
    public interface IBaseSetpoints
    {
        #region Индекс уставки
        /// <summary>
        /// Индекс уставки
        /// </summary>
        string Index { get; set; } 
        #endregion

        #region Идентификатор
        /// <summary>
        /// Идентификатор
        /// </summary>
        string Id { get; set; }
        #endregion

        #region Описание уставки
        /// <summary>
        /// Описание уставки
        /// </summary>
        string Description { get; set; }
        #endregion

        #region Имя переменной
        /// <summary>
        /// Имя переменной
        /// </summary>
        string VarName { get; set; }
        #endregion

        #region Адрес
        /// <summary>
        /// Адрес
        /// </summary>
        string Address { get; set; }
        #endregion

        #region Значение уставки
        /// <summary>
        /// Значение уставки
        /// </summary>
        string Value { get; set; }
        #endregion

        #region Ед. измерения
        /// <summary>
        /// Ед. измерения
        /// </summary>
        string Unit { get; set; }
        #endregion
    }
}